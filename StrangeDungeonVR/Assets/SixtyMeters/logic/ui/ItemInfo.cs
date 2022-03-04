using HurricaneVR.Framework.Core;
using TMPro;
using UnityEngine;

namespace SixtyMeters.logic.ui
{
    public class ItemInfo : MonoBehaviour
    {
        // Settings
        public bool showWhileHovering;
        public bool showWhileHolding;

        // Components
        public HVRGrabbable grabbable;
        public Canvas canvas;
        public TextMeshProUGUI tmpTitle;
        public TextMeshProUGUI tmpDescription;


        // Internals
        private Transform _playerCameraTransform;

        // Start is called before the first frame update
        void Start()
        {
            if (showWhileHovering)
            {
                grabbable.HoverEnter.AddListener((_, _) => { canvas.gameObject.SetActive(true); });
                grabbable.HoverExit.AddListener((_, _) =>
                {
                    if (!grabbable.IsHandGrabbed)
                    {
                        canvas.gameObject.SetActive(false);
                    }
                });
            }

            if (showWhileHolding)
            {
                grabbable.Grabbed.AddListener((_, _) => { canvas.gameObject.SetActive(true); });
                grabbable.Released.AddListener((_, _) => { canvas.gameObject.SetActive(false); });
            }

            _playerCameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
            RotateTextToFacePlayer();
        }

        //TODO: move onto canvas script
        private void RotateTextToFacePlayer()
        {
            canvas.transform.rotation =
                Quaternion.LookRotation(canvas.transform.position - _playerCameraTransform.position);
        }

        public void SetTitle(string title)
        {
            tmpTitle.text = title;
        }

        public void SetDescription(string description)
        {
            tmpDescription.text = description;
        }
    }
}