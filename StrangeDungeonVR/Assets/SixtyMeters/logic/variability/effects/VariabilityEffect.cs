using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core;
using SixtyMeters.logic.analytics;
using SixtyMeters.logic.generator;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.ui;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.variability.effects
{
    public abstract class VariabilityEffect : MonoBehaviour, IConsumable
    {
        // Internal components
        protected VariabilityManager VariabilityManager;
        private StatisticsManager _statistics;
        private DungeonGenerator _dungeonGenerator;
        private ItemInfo _itemInfo;
        private SoulShardHelper _soulShardHelper;
        private HVRGrabbable _grabbable;
        private VibrationHelper _vibrationHelper;

        // Internals
        private bool _inRangeForConsumption;

        void Start()
        {
            VariabilityManager = FindObjectOfType<VariabilityManager>();
            _statistics = FindObjectOfType<StatisticsManager>();
            _vibrationHelper = new VibrationHelper(FindObjectOfType<HVRInputManager>());
            _itemInfo = GetComponent<ItemInfo>();
            _soulShardHelper = GetComponent<SoulShardHelper>();
            _grabbable = GetComponent<HVRGrabbable>();
            _dungeonGenerator = FindObjectOfType<DungeonGenerator>();

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

            //Re-parent to current center tile to avoid despawning of shards looted from vanishing destructables
            gameObject.transform.parent = _dungeonGenerator.GetCurrentCenterTile().transform;

            ++_statistics.soulShardsFound;
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
            ++_statistics.soulShardsUsed;

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

            _grabbable.HandGrabbers.ForEach(grabber =>
            {
                _vibrationHelper.VibrateHand(grabber.HandSide,
                    inRange ? VibrationHelper.InRangeVibration : VibrationHelper.OutOfRangeVibration);
            });
        }
    }
}