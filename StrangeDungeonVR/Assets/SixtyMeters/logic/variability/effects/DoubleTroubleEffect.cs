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
                   "<color=green>EFFECT: Damage dealt by the bearer is doubled. </color>" +
                   "<color=red>Damage taken by the bearer is also doubled.</color>\n\n" +
                   "Pull towards your heart to use.";
        }

        protected override void ApplyEffectImplementation()
        {
            VariabilityManager.player.damageDealtMultiplier *= 2;
            VariabilityManager.player.damageTakenMultiplier *= 2;
        }

        protected override void RemoveEffectImplementation()
        {
            VariabilityManager.player.damageDealtMultiplier %= 2;
            VariabilityManager.player.damageTakenMultiplier %= 2;
        }
    }
}