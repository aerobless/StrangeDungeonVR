using HurricaneVR.Framework.Core.UI;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.ui
{
    public class RegisterInputCanvas : MonoBehaviour
    {
        private HVRInputModule _inputModule;
        private Canvas _canvas;

        // Start is called before the first frame update
        void Start()
        {
            _inputModule = FindObjectOfType<GameManager>().uiManager;
            _canvas = GetComponent<Canvas>();
            _inputModule.AddCanvas(_canvas);
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy called on RegisterInputCanvas !!");
            _inputModule.RemoveCanvas(_canvas);
        }
    }
}