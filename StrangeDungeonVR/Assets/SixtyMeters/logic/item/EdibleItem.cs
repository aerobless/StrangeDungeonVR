using System.Collections.Generic;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Core;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.ui;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.item
{
    public abstract class EdibleItem : PlayerItem, IConsumable
    {
        public AudioSource audioSource;

        // Settings
        [Tooltip("e.g. uncorking a bottle to drink from")]
        public List<AudioClip> prepareClips;

        [Tooltip("e.g. drinking from a bottle")]
        public List<AudioClip> consumeClips;

        [Tooltip("how often can the item be used until its spent")]
        public int usesLeft;

        // Internal components
        private HVRGrabbable _grabbable;
        protected GameManager GameManager;
        private ItemInfo _itemInfo;

        // Internals
        private bool _inRangeForConsumption;

        // Start is called before the first frame update
        void Start()
        {
            _grabbable = GetComponent<HVRGrabbable>();
            _itemInfo = GetComponent<ItemInfo>();
            GameManager = FindObjectOfType<GameManager>();

            InitImplementation();

            if (_itemInfo)
            {
                _itemInfo.SetTitle(GetFullName());
                _itemInfo.SetDescription(GetDescription());
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public abstract void InitImplementation();

        public abstract string GetFullName();

        public abstract string GetEmptyName();
        public abstract string GetDescription();

        protected abstract void ApplyEffectImplementation();

        private void ApplyEffect()
        {
            StartCoroutine(Helper.PlaySound(audioSource, prepareClips, 0.6f,
                () => { StartCoroutine(Helper.PlaySound(audioSource, consumeClips, () => { })); }));

            ApplyEffectImplementation();

            --usesLeft;

            if (usesLeft <= 0)
            {
                _itemInfo.SetTitle(GetEmptyName());
                //TODO: replace with used bottle skin   
            }
        }

        public void InRangeForConsumption(bool inRange)
        {
            _inRangeForConsumption = inRange;

            if (usesLeft > 0)
            {
                GameManager.controllerFeedback.VibrateHand(_grabbable, inRange
                    ? ControllerFeedbackHelper.InRangeVibration
                    : ControllerFeedbackHelper.OutOfRangeVibration);

                ApplyEffect();
            }
        }
    }
}