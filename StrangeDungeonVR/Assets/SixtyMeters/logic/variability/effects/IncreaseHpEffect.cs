namespace SixtyMeters.logic.variability.effects
{
    public class IncreaseHpEffect : VariabilityEffect
    {
        public override string GetName()
        {
            return "Fortune of the Brave";
        }

        public override string GetDescription()
        {
            return "The dungeon rewards brave adventurers for their toils.\n\n" +
                   "<color=green>EFFECT: Increases your HP by 10</color>\n\n" +
                   "Pull towards your heart to use.";
        }

        protected override void ApplyEffectImplementation()
        {
            VariabilityManager.player.baseHealth += 10;
        }

        protected override void RemoveEffectImplementation()
        {
            VariabilityManager.player.baseHealth -= 10;
        }
    }
}