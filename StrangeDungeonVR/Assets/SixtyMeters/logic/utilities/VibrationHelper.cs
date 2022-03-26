using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Shared;

namespace SixtyMeters.logic.utilities
{
    public class VibrationHelper
    {
        private readonly HVRInputManager _inputManager;

        public VibrationHelper(HVRInputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public static readonly HapticData InRangeVibration =
            new() {Amplitude = 0.2f, Duration = 10f, Frequency = 0.8f};

        public static readonly HapticData OutOfRangeVibration =
            new() {Amplitude = 0.8f, Duration = 0.2f, Frequency = 0.8f};


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
    }
}