using UnityEngine;

namespace SixtyMeters.logic.fx
{
    public class FollowGroundFx : MonoBehaviour
    {
        // Components
        public new Rigidbody rigidbody;
        public GameObject followGroundObject;

        // Settings
        public float velocityThreshold;

        // Internals
        private Transform _groundObjectTransform;

        // Start is called before the first frame update
        void Start()
        {
            _groundObjectTransform = followGroundObject.transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (rigidbody.velocity.magnitude >= velocityThreshold)
            {
                followGroundObject.SetActive(true);

                // Reset rotation, because we don't want our fx to rotate with the main object
                _groundObjectTransform.localRotation = Quaternion.Euler(0, 0, 0);

                if (Physics.Raycast(_groundObjectTransform.position, -Vector3.up, out var hit, 2f))
                {
                    var localHit = _groundObjectTransform.InverseTransformPoint(hit.point);
                    _groundObjectTransform.localPosition = new Vector3(0, localHit.y-0.5f, 0);
                }
                else
                {
                    followGroundObject.SetActive(false);
                }
            }
            else
            {
                followGroundObject.SetActive(false);
            }
        }
    }
}