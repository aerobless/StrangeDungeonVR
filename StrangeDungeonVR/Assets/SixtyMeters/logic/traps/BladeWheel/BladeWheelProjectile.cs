using System.Collections;
using UnityEngine;

namespace SixtyMeters.logic.traps.BladeWheel
{
    public class BladeWheelProjectile : MonoBehaviour
    {
        // Settings
        public float initialTorque;
        public float applyForceTime;

        // Internal
        private Rigidbody _rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            StartCoroutine(StopApplyingTorque());
        }

        // Update is called once per frame
        void Update()
        {
        }

        private IEnumerator StopApplyingTorque()
        {
            yield return new WaitForSeconds(applyForceTime);
            initialTorque = 0;
        }

        void FixedUpdate()
        {
            _rigidbody.AddTorque(new Vector3(initialTorque, 0, 0));
        }
    }
}