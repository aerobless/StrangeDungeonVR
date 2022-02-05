using System;
using SixtyMeters.logic.player;
using UnityEngine;

namespace SixtyMeters.logic.door
{
    public class DungeonTileConnection : MonoBehaviour
    {
        // Used to find compatible tile connections
        public string connectionId;

        private TileConnectionState _state = TileConnectionState.UNATTACHED;
        private DungeonTile _parentTile;
        private DungeonTileConnection _connectedDoor;

        // Start is called before the first frame update
        void Start()
        {
            _parentTile = GetComponentInParent<DungeonTile>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public bool IsAttached()
        {
            return _state == TileConnectionState.ATTACHED;
        }

        public bool IsUnattached()
        {
            return _state == TileConnectionState.UNATTACHED;
        }

        public bool IsLocked()
        {
            return _state == TileConnectionState.LOCKED;
        }


        public void Attach(DungeonTileConnection doorInAttachedTile)
        {
            _connectedDoor = doorInAttachedTile;
            _state = TileConnectionState.ATTACHED;
        }

        /// <summary>
        /// Returns the tile attached to this door
        /// </summary>
        /// <returns></returns>
        public DungeonTile GetAttachedTile()
        {
            return _connectedDoor.GetParentTile();
        }

        public DungeonTileConnection GetAttachedDoor()
        {
            return _connectedDoor;
        }

        /// <summary>
        /// Get the parent dungeon tile of this door
        /// </summary>
        /// <returns></returns>
        public DungeonTile GetParentTile()
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
            Gizmos.DrawCube(Vector3.zero + new Vector3(0, 0, -3f), new Vector3(0.1f, 0.1f, 3f));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerOneBodyCollider>())
            {
                _parentTile.NotifyPlayerEnterOrExit();
            }
        }

        /// <summary>
        /// Should be called when the attached tile is removed, locks the door permanently
        /// </summary>
        public void Lock()
        {
            _state = TileConnectionState.LOCKED;
        }
    }
}