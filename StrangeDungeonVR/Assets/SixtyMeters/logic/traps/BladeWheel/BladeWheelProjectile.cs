using System.Collections;
using SixtyMeters.logic.fighting;
using UnityEngine;

namespace SixtyMeters.logic.traps.BladeWheel
{
    public class BladeWheelProjectile : MonoBehaviour
    {
        //Components
        public new Rigidbody rigidbody;
        public GameObject bladeWheelBody;
        public GameObject bladeWheelFxSparks;
        public DamageObject damageObject;

        // Settings
        public float initialTorque;
        public float applyForceTime;
        public float sparksFxVelocityThreshold;
        public float destroyAfterSeconds;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StopApplyingTorque());
            Destroy(gameObject, destroyAfterSeconds);
        }


        // Update is called once per frame
        void Update()
        {
            if (rigidbody.velocity.magnitude >= sparksFxVelocityThreshold)
            {
                bladeWheelFxSparks.SetActive(true);

                bladeWheelFxSparks.transform.position = bladeWheelBody.transform.position + new Vector3(0, -0.8f, 0);
                damageObject.enabled = true;
            }
            else
            {
                bladeWheelFxSparks.SetActive(false);
                damageObject.enabled = false;
            }
        }

        private IEnumerator StopApplyingTorque()
        {
            yield return new WaitForSeconds(applyForceTime);
            initialTorque = 0;
        }

        void FixedUpdate()
        {
            rigidbody.AddTorque(transform.right * initialTorque);
        }
    }
}