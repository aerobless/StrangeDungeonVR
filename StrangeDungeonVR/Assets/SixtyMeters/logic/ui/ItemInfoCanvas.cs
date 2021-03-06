using TMPro;
using UnityEngine;

namespace SixtyMeters.logic.ui
{
    public class ItemInfoCanvas : MonoBehaviour
    {
        private Canvas _canvas;
        private Transform _playerCameraTransform;

        // Settings
        public bool rotateCanvasToFacePlayer;
        public bool floatAboveItem;

        // Components
        public TextMeshProUGUI tmpTitle;
        public TextMeshProUGUI tmpDescription;

        // Start is called before the first frame update
        void Start()
        {
            _canvas = GetComponent<Canvas>();
            _playerCameraTransform = Camera.main.transform;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void LateUpdate()
        {
            if (rotateCanvasToFacePlayer)
            {
                RotateCanvasToFacePlayer();
            }
        }

        private void RotateCanvasToFacePlayer()
        {
            if (floatAboveItem)
            {
                _canvas.transform.position = gameObject.transform.parent.position + (Vector3.up * 0.5f);
            }

            _canvas.transform.rotation =
                Quaternion.LookRotation(_canvas.transform.position - _playerCameraTransform.position);
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