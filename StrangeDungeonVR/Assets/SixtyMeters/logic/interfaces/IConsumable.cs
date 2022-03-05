namespace SixtyMeters.logic.interfaces
{
    public interface IConsumable
    {
        /// <summary>
        /// Called when the consumable is in range to be consumed. Actual consumption occurs when the player releases
        /// the item from their grip.
        /// </summary>
        public void InRangeForConsumption(bool inRange);
    }
}