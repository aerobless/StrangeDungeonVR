using System.Collections;
using RootMotion.Dynamics;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class PhysicalAgentHitbox : MonoBehaviour
    {
        // Internal Settings
        private readonly float _minVelocityForDamage = 2;

        // Internal Dynamics
        private IDamageable _dmgListener;
        private PuppetMaster _puppetMaster;

        // Settings
        public bool unpinMuscles;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SetupHitbox(IDamageable listener, PuppetMaster puppetMaster)
        {
            _dmgListener = listener;
            _puppetMaster = puppetMaster;
        }

        // Used for detecting damage by traps etc.
        private void OnCollisionEnter(Collision other)
        {
            var damageObject = other.gameObject.GetComponent<DamageObject>();
            if (damageObject && damageObject.enabled)
            {
                //Vector3 collisionForce = other.impulse / Time.fixedDeltaTime;
                var relativeVelocityMagnitude = other.relativeVelocity.magnitude;
                if (relativeVelocityMagnitude > _minVelocityForDamage)
                {
                    var impactPoint = other.GetContact(0).point;
                    _dmgListener.ApplyDamage(damageObject.GetDamagePoints(), relativeVelocityMagnitude, impactPoint);

                    if (unpinMuscles)
                    {
                        //Unpin muscles
                        var muscle = _puppetMaster.GetMuscleIndex(GetComponent<Rigidbody>());
                        _puppetMaster.SetMuscleWeightsRecursive(muscle, 0.025f, 0f);
                        StartCoroutine(ResetMuscle(muscle));
                        
                    }
                    
                    // Apply force
                    Vector3 dir = other.contacts[0].point - transform.position;
                    dir.Normalize();

                    // var magnitude = 10;
                    // We then get the opposite (-Vector3) and normalize it
                    //dir = -dir.normalized;
                    // And finally we add force in the direction of dir and multiply it by force. 
                    // This will push back the player
                    GetComponent<Rigidbody>().AddForce(dir * relativeVelocityMagnitude * 150);
                }
            }
        }

        IEnumerator ResetMuscle(int muscle)
        {
            yield return new WaitForSeconds(2);
            _puppetMaster.SetMuscleWeightsRecursive(muscle, 0.8f, 0.8f);
        }
    }
}