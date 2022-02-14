using System.Collections;
using System.Collections.Generic;
using RootMotion.Dynamics;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class AgentHitbox : MonoBehaviour
    {
        private HumanoidAgentDamageable _dmgListener;
        private PuppetMaster _puppetMaster;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SetupHitbox(HumanoidAgentDamageable listener, PuppetMaster puppetMaster)
        {
            _dmgListener = listener;
            _puppetMaster = puppetMaster;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<DamageObject>())
            {
                _dmgListener.ApplyDamage(other.gameObject.GetComponent<DamageObject>().GetDamage());
                //TODO: reset after a while
                Debug.Log(name);
                
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


        IEnumerator ResetMuscle(int muscle)
        {
            yield return new WaitForSeconds(2);
            _puppetMaster.SetMuscleWeightsRecursive(muscle, 0.8f, 0.8f);
            yield return null;
        }
    }
}