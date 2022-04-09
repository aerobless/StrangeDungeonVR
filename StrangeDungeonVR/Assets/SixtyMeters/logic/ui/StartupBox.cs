using HurricaneVR.Framework.Core.Player;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.ui
{
    public class StartupBox : MonoBehaviour
    {
        private GameManager _gameManager;

        // Components
        public AudioSource audioSource;
        public AudioClip clickButton;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        public void PlaySitting()
        {
            var hvrCameraRig = GetCameraRig();
            hvrCameraRig.SetSitStandMode(HVRSitStand.Sitting);
            CalibrateAndDestroy(hvrCameraRig);
        }

        public void PlayStanding()
        {
            var hvrCameraRig = GetCameraRig();
            hvrCameraRig.SetSitStandMode(HVRSitStand.Standing);
            CalibrateAndDestroy(hvrCameraRig);
        }

        private HVRCameraRig GetCameraRig()
        {
            return _gameManager.player.GetComponentInChildren<HVRCameraRig>();
        }

        private void CalibrateAndDestroy(HVRCameraRig cameraRig)
        {
            cameraRig.Calibrate();
            audioSource.PlayOneShot(clickButton);
            Destroy(gameObject, 1f);
        }
    }
}