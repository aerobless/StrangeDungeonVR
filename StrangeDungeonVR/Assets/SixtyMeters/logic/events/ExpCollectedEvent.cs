namespace SixtyMeters.logic.events
{
    public class ExpCollectedEvent
    {
        // The total exp collected in this collection period, e.g. current level
        // normalized between 0 and 1, useful for a slider/graphic
        public readonly float totalExpCollectedNormalized;

        public ExpCollectedEvent(float totalExpCollectedNormalized)
        {
            this.totalExpCollectedNormalized = totalExpCollectedNormalized;
        }
    }
}