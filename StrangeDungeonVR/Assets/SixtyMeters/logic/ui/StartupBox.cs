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
        public AudioClip spawnButton;

        public GameObject heightCalibrationPage;
        public GameObject controllerExplanationPage;
        public GameObject boxContent;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            heightCalibrationPage.SetActive(true);
            controllerExplanationPage.SetActive(false);
        }

        public void PlaySitting()
        {
            var hvrCameraRig = GetCameraRig();
            hvrCameraRig.SetSitStandMode(HVRSitStand.Sitting);
            Calibrate(hvrCameraRig);
        }

        public void PlayStanding()
        {
            var hvrCameraRig = GetCameraRig();
            hvrCameraRig.SetSitStandMode(HVRSitStand.Standing);
            Calibrate(hvrCameraRig);
        }

        public void EndStartupBox()
        {
            audioSource.PlayOneShot(spawnButton);
            boxContent.SetActive(false);
            Destroy(gameObject, 3f);
        }

        private HVRCameraRig GetCameraRig()
        {
            return _gameManager.player.GetComponentInChildren<HVRCameraRig>();
        }

        private void Calibrate(HVRCameraRig cameraRig)
        {
            cameraRig.Calibrate();
            audioSource.PlayOneShot(clickButton);
            heightCalibrationPage.SetActive(false);
            controllerExplanationPage.SetActive(true);
        }
    }
}