namespace SixtyMeters.logic.door
{
    public enum TileConnectionState
    {
        UNATTACHED, // A fresh door without a connecting tile yet
        ATTACHED, // A door that's attached to a connecting tile
        LOCKED // A door that used to be connected but, it's connecting tile has since been deleted
    }
}