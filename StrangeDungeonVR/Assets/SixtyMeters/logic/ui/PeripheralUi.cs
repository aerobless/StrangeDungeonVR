using UnityEngine;

namespace SixtyMeters.logic.ui
{
    /// <summary>
    /// Used to have certain UI elements always in view of the player but not bound to their camera.
    /// This allows the players to look around unobstructed, but always have UI elements within glance range.
    /// </summary>
    public class PeripheralUi : MonoBehaviour
    {
        [Header("Required Transforms")] 
        public Transform PlayerController;
        public Transform Camera;

        [Header("Settings")] [Tooltip("The UI will be position this much lower than the camera.")]
        public float CameraOffset = .6f;

        [Tooltip(
            "If the delta between the camera forward and UI forward is greater than this, the waist will rotate at WaistSpeed")]
        public float WaistAngleThreshold = 70;

        [Tooltip("Speed of the UI catchup when too far from the camera gaze")]
        public float WaistSpeed = 90f;

        public void Update()
        {
            FollowPlayer();
        }

        public void FollowPlayer()
        {
            var waistPosition = PlayerController.position;
            waistPosition.y = Camera.position.y - CameraOffset;
            transform.position = waistPosition;

            var from = Camera.forward;
            @from.y = 0f;
            var angle = Vector3.SignedAngle(@from, Camera.forward, Camera.right);

            angle = Vector3.Angle(Camera.forward, transform.forward);
            if (angle > WaistAngleThreshold)
            {
                var waistRotation = Quaternion
                    .RotateTowards(transform.rotation, Camera.rotation, WaistSpeed * Time.deltaTime).eulerAngles;
                waistRotation.x = 0f;
                waistRotation.z = 0f;
                transform.eulerAngles = waistRotation;
            }
        }

        public void ResetVision()
        {
            transform.rotation = Quaternion.Euler(0.0f, Camera.rotation.eulerAngles.y, 0.0f);
        }
    }
}