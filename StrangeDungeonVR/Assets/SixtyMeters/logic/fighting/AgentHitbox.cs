using System.Collections;
using RootMotion.Dynamics;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class AgentHitbox : MonoBehaviour
    {
        // Internal Settings
        private readonly float _minVelocityForDamage = 2;

        // Internal Dynamics
        private IDamageable _dmgListener;
        private PuppetMaster _puppetMaster;

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
                    _dmgListener.ApplyDamage(damageObject, relativeVelocityMagnitude, impactPoint);

                    // Apply force
                    Vector3 dir = other.contacts[0].point - transform.position;
                    dir.Normalize();

                    var magnitude = 5000;
                    // We then get the opposite (-Vector3) and normalize it
                    //dir = -dir.normalized;
                    // And finally we add force in the direction of dir and multiply it by force. 
                    // This will push back the player
                    GetComponent<Rigidbody>().AddForce(dir * magnitude);

                    //Unpin muscles
                    //var muscle = _puppetMaster.GetMuscleIndex(GetComponent<Rigidbody>());
                    //_puppetMaster.SetMuscleWeightsRecursive(muscle, 0.5f, 0f);
                    //StartCoroutine(ResetMuscle(muscle));
                }
            }
        }
        
        IEnumerator ResetMuscle(int muscle)
        {
            yield return new WaitForSeconds(2);
            _puppetMaster.SetMuscleWeightsRecursive(muscle, 0.8f, 0.8f);
            yield return null;
        }
    }
}