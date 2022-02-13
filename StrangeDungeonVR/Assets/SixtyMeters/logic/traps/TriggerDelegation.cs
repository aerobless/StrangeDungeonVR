using UnityEngine;
using UnityEngine.Events;

namespace SixtyMeters.logic.traps
{
    public class TriggerDelegation : MonoBehaviour
    {
        public UnityEvent<Collider> doOnTriggerEnter = new();

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            doOnTriggerEnter.Invoke(other);
        }
    }
}