namespace SixtyMeters.logic.variability.effects
{
    public class IncreaseDamageEffect : VariabilityEffect
    {
        public override string GetName()
        {
            return "Lesser strength";
        }

        public override string GetDescription()
        {
            return "You should really take better care of your muscles!\n\n" +
                   "<color=green>EFFECT: Damage dealt with swords is increased by 5.</color>\n\n" +
                   "Pull towards your heart to use.";
        }

        protected override void ApplyEffectImplementation()
        {
            VariabilityManager.player.swordBaseDamage += 5;
        }

        protected override void RemoveEffectImplementation()
        {
            VariabilityManager.player.swordBaseDamage -= 5;
        }
    }
}