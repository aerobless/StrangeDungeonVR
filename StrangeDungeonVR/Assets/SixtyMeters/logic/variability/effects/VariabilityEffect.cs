using SixtyMeters.logic.ui;
using UnityEngine;

namespace SixtyMeters.logic.variability.effects
{
    public abstract class VariabilityEffect : MonoBehaviour
    {
        protected VariabilityManager VariabilityManager;
        private ItemInfo _itemInfo;

        void Start()
        {
            VariabilityManager = FindObjectOfType<VariabilityManager>();
            _itemInfo = GetComponent<ItemInfo>();

            if (_itemInfo)
            {
                _itemInfo.SetTitle(GetName());
                _itemInfo.SetDescription(GetDescription());
            }
        }


        public abstract string GetName();

        public abstract string GetDescription();

        protected abstract void ApplyEffectImplementation();

        protected abstract void RemoveEffectImplementation();

        public void ApplyEffect()
        {
            ApplyEffectImplementation();
            VariabilityManager.AddAppliedEffect(this);
            Destroy(gameObject);
        }

        public void RemoveEffect()
        {
            RemoveEffectImplementation();
            VariabilityManager.RemoveAppliedEffect(this);
        }
    }
}