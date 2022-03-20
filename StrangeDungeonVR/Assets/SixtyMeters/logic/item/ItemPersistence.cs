namespace SixtyMeters.logic.item
{
    public enum ItemPersistence
    {
        None, // No special handling, item is destroyed with parent
        Room, // Item is destroyed when the currently active room is destroyed
        Persistent // The will not be destroyed unless it falls out of the map
    }
}