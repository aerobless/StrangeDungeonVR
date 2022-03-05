using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Shared;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.ui;
using UnityEngine;

namespace SixtyMeters.logic.variability.effects
{
    public abstract class VariabilityEffect : MonoBehaviour, IConsumable
    {
        // Internal components
        protected VariabilityManager VariabilityManager;
        private ItemInfo _itemInfo;
        private SoulShardHelper _soulShardHelper;
        private HVRGrabbable _grabbable;

        private HVRInputManager _inputManager;

        private readonly HapticData _inRangeVibration = new() {Amplitude = 0.2f, Duration = 10f, Frequency = 0.8f};
        private readonly HapticData _outOfRangeVibration = new() {Amplitude = 0.8f, Duration = 0.2f, Frequency = 0.8f};

        // Internals
        private bool _inRangeForConsumption;

        void Start()
        {
            VariabilityManager = FindObjectOfType<VariabilityManager>();
            _inputManager = FindObjectOfType<HVRInputManager>();
            _itemInfo = GetComponent<ItemInfo>();
            _soulShardHelper = GetComponent<SoulShardHelper>();
            _grabbable = GetComponent<HVRGrabbable>();

            if (_itemInfo)
            {
                _itemInfo.SetTitle(GetName());
                _itemInfo.SetDescription(GetDescription());
            }

            if (_grabbable)
            {
                _grabbable.Released.AddListener((_, _) =>
                {
                    if (_inRangeForConsumption)
                    {
                        ApplyEffect();
                    }
                });
            }
        }


        public abstract string GetName();

        public abstract string GetDescription();

        protected abstract void ApplyEffectImplementation();

        protected abstract void RemoveEffectImplementation();

        private void ApplyEffect()
        {
            _soulShardHelper.ApplySoulShard();
            ApplyEffectImplementation();
            VariabilityManager.AddAppliedEffect(this);

            // Immediately destroy the visible soul shard
            Destroy(_grabbable);
            Destroy(_itemInfo.canvas.gameObject);
            Destroy(gameObject.GetComponent<MeshRenderer>());

            // Destroy the actual gameObject after the sound has finished playing
            Destroy(gameObject, 1.8f);
        }

        public void RemoveEffect()
        {
            RemoveEffectImplementation();
            VariabilityManager.RemoveAppliedEffect(this);
        }

        public void InRangeForConsumption(bool inRange)
        {
            _inRangeForConsumption = inRange;
            VibrateHand(inRange ? _inRangeVibration : _outOfRangeVibration);
        }

        private void VibrateHand(HapticData haptics)
        {
            if (_grabbable.IsLeftHandGrabbed)
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