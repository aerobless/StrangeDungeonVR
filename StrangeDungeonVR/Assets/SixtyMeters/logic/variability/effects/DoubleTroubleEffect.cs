using UnityEngine;

namespace SixtyMeters.logic.variability.effects
{
    public class DoubleTroubleEffect : VariabilityEffect
    {
        public override string GetName()
        {
            return "Double Trouble";
        }

        public override string GetDescription()
        {
            return "Invented by Mage Flavorius to deal with invaders.\n\n" +
                   "<color=green>EFFECT: Damage dealt by the bearer is doubled. Damage taken by the bearer is also doubled.";
        }

        protected override void ApplyEffectImplementation()
        {
            VariabilityManager.damageDealtMultiplier *= 2;
            VariabilityManager.damageTakenMultiplier *= 2;
        }

        protected override void RemoveEffectImplementation()
        {
            VariabilityManager.damageDealtMultiplier %= 2;
            VariabilityManager.damageTakenMultiplier %= 2;
        }
    }
}