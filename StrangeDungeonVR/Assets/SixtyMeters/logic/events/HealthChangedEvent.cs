namespace SixtyMeters.logic.events
{
    public class HealthChangedEvent
    {
        public readonly float DamageTakenAmountPerEvent;

        public readonly float NewHealth;
        public readonly float NewHealthNormalized;

        public readonly float MaxHealth;

        public HealthChangedEvent(float damageTakenAmountPerEvent, float newHealth, float newHealthNormalized, float maxHealth)
        {
            DamageTakenAmountPerEvent = damageTakenAmountPerEvent;
            NewHealth = newHealth;
            NewHealthNormalized = newHealthNormalized;
            MaxHealth = maxHealth;
        }
    }
}