using System;
using HurricaneVR.Framework.Core.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SixtyMeters.logic.utilities
{
    public class VelocityAction : MonoBehaviour
    {
        public new Rigidbody rigidbody;
        public float velocityThreshold;
        public VelocityChangeUpdate afterVelocityDropsBelowThreshold = new VelocityChangeUpdate();

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (rigidbody.velocity.magnitude < velocityThreshold)
            {
                afterVelocityDropsBelowThreshold.Invoke();
            }
        }

        [Serializable]
        public class VelocityChangeUpdate : UnityEvent
        {
        }
    }
}