using HurricaneVR.Framework.Core;
using UnityEngine;

namespace SixtyMeters.logic.ui
{
    public class ItemInfo : MonoBehaviour
    {
        // Settings
        public bool showWhileHovering;
        public bool showWhileHolding;
        public bool rotateTooltipToFacePlayer;
        public bool floatAboveItem;

        // Components
        public HVRGrabbable grabbable;
        public ItemInfoCanvas canvas;

        // Start is called before the first frame update
        void Start()
        {
            canvas.gameObject.SetActive(false);
            
            if (showWhileHovering)
            {
                grabbable.HoverEnter.AddListener((_, _) =>
                {
                    if (!grabbable.IsHandGrabbed)
                    {
                        canvas.gameObject.SetActive(true);
                    }
                });
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

            // Delegation
            canvas.floatAboveItem = floatAboveItem;
            canvas.rotateCanvasToFacePlayer = rotateTooltipToFacePlayer;
        }

        public void SetTitle(string title)
        {
            canvas.SetTitle(title);
        }

        public void SetDescription(string description)
        {
            canvas.SetDescription(description);
        }
    }
}