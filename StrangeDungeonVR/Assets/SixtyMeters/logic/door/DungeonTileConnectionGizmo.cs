using SixtyMeters.logic.generator;
using UnityEngine;

namespace SixtyMeters.logic.door
{
    /// <summary>
    /// Used to perfectly align two dungeon tiles. Has a state whether a tile has been attached or not.
    /// Should be considered a child of DungeonTileConnection. These two classes are kept separate to make it possible
    /// to align the outer gameObject (DungeonTileConnection) with the grid system, and the inner gameObject (DungeonTileConnection)
    /// with the level.
    /// </summary>
    public class DungeonTileConnectionGizmo : MonoBehaviour
    {
        private TileConnectionState _state = TileConnectionState.Unattached;
        private DungeonTile _parentTile;
        private DungeonTileConnectionGizmo _connectedDoor;

        // Internals
        private bool _handshakeOccured;

        // Start is called before the first frame update
        void Start()
        {
            _parentTile = GetComponentInParent<DungeonTile>();
        }

        public bool IsAttached()
        {
            return _state == TileConnectionState.Attached;
        }

        public bool IsUnattached()
        {
            return _state == TileConnectionState.Unattached;
        }

        public void Attach(DungeonTileConnectionGizmo doorInAttachedTile)
        {
            _connectedDoor = doorInAttachedTile;
            _state = TileConnectionState.Attached;
        }

        /// <summary>
        /// Returns the tile attached to this door
        /// </summary>
        /// <returns></returns>
        public DungeonTile GetAttachedTile()
        {
            return _connectedDoor.GetParentTile();
        }

        public DungeonTileConnectionGizmo GetAttachedDoor()
        {
            return _connectedDoor;
        }

        /// <summary>
        /// Get the parent dungeon tile of this door
        /// </summary>
        /// <returns></returns>
        private DungeonTile GetParentTile()
        {
            if (_parentTile == null)
            {
                _parentTile = GetComponentInParent<DungeonTile>();
            }

            return _parentTile;
        }

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.DrawLine(Vector3.zero, Vector3.forward);

            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero + new Vector3(0, 0, -2f), new Vector3(0.1f, 0.1f, 2f));
        }

        public void EnterTile()
        {
            _parentTile.NotifyPlayerEnter();
        }

        public void ExitTile()
        {
            _parentTile.NotifyPlayerExit();
        }
    }
}