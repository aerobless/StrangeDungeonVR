using UnityEngine;

namespace SixtyMeters.logic.props
{
    public class LockRotationAxis : MonoBehaviour
    {
        public Quaternion lockedRotation;

        private void LateUpdate()
        {
            transform.rotation = lockedRotation;
        }
    }
}