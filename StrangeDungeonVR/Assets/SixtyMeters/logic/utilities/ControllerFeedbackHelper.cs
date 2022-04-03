using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Shared;

namespace SixtyMeters.logic.utilities
{
    public class ControllerFeedbackHelper
    {
        private readonly HVRInputManager _inputManager;

        public ControllerFeedbackHelper(HVRInputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public static readonly HapticData InRangeVibration =
            new() {Amplitude = 0.2f, Duration = 10f, Frequency = 0.8f};

        public static readonly HapticData OutOfRangeVibration =
            new() {Amplitude = 0.8f, Duration = 0.2f, Frequency = 0.8f};
        
        public static readonly HapticData ImpactVibration =
            new() {Amplitude = 0.9f, Duration = 0.3f, Frequency = 0.4f};


        /// <summary>
        /// Vibrates the specified hand with the provided haptic data.
        /// </summary>
        /// <param name="hand">the hand to be vibrated</param>
        /// <param name="haptics">the vibration information</param>
        public void VibrateHand(HVRHandSide hand, HapticData haptics)
        {
            if (hand == HVRHandSide.Left)
            {
                _inputManager.LeftController.Vibrate(haptics);
            }
            else
            {
                _inputManager.RightController.Vibrate(haptics);
            }
        }

        /// <summary>
        /// Vibrates all hands attached to the given grabbable with the provided haptic data.
        /// </summary>
        /// <param name="grabbable">the grabbable for which the hands should be vibrated</param>
        /// <param name="haptics">the vibration information</param>
        public void VibrateHand(HVRGrabbable grabbable, HapticData haptics)
        {
            grabbable.HandGrabbers.ForEach(grabber => { VibrateHand(grabber.HandSide, haptics); });
        }
    }
}