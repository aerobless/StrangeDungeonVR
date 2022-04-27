using UnityEngine;
using UnityEngine.Events;

namespace SixtyMeters.logic.utilities
{
    public class CollisionDelegation : MonoBehaviour
    {
        public UnityEvent<Collision> onCollisionEnter = new();

        private void OnCollisionEnter(Collision other)
        {
            onCollisionEnter.Invoke(other);
        }
    }
}