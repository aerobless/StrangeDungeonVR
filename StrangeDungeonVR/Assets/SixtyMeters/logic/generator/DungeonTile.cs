using System.Collections.Generic;
using System.Linq;
using SixtyMeters.logic.analytics;
using SixtyMeters.logic.door;
using SixtyMeters.logic.interfaces;
using UnityEngine;

namespace SixtyMeters.logic.generator
{
    public class DungeonTile : MonoBehaviour
    {
        public List<DungeonTileConnection> tileDoors;

        // Set when this tile is instantiated by the generator
        public int tileSeed;
        public bool isStartTile;

        // The current center of the game, moves with the player
        private bool _tileIsOccupiedByPlayer = false;

        // Internal components
        private DungeonGenerator _dungeonGenerator;
        private StatisticsManager _statistics;

        // Start is called before the first frame update
        void Start()
        {
            _dungeonGenerator = FindObjectOfType<DungeonGenerator>();
            _statistics = FindObjectOfType<StatisticsManager>();

            var randomizedElements = GetComponentsInChildren<IRandomizeable>();
            foreach (var go in randomizedElements)
            {
                go.Randomize();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public List<DungeonTile> GetAttachedTiles()
        {
            return tileDoors.Where(door => door.connection.IsAttached())
                .Select(door => door.connection.GetAttachedTile())
                .ToList();
        }

        public void NotifyPlayerEnter()
        {
            if (!_tileIsOccupiedByPlayer)
            {
                Debug.Log("Player has entered tile " + gameObject.name);
                _tileIsOccupiedByPlayer = true;
                _dungeonGenerator.TileEntered(this);

                // Statistics
                _statistics.StartTrackingIfNotStartedYet();
                ++_statistics.roomsDiscovered;
            }
        }

        public void NotifyPlayerExit()
        {
            if (_tileIsOccupiedByPlayer)
            {
                Debug.Log("Player has left tile " + gameObject.name);
            }

            _tileIsOccupiedByPlayer = false;
        }

        public List<DungeonTileConnectionGizmo> GetAttachedDoors()
        {
            return tileDoors.Where(door => door.connection.IsAttached())
                .Select(door => door.connection.GetAttachedDoor())
                .ToList();
        }

        public List<DungeonTileConnection> GetEntranceDoorsForArea(DungeonArea area)
        {
            var allowedTypes = new List<TileConnectionType>
            {
                TileConnectionType.EntranceOnly, TileConnectionType.EntranceAndExit
            };
            return tileDoors
                .Where(door => door.dungeonArea.Equals(area) && allowedTypes.Contains(door.tileConnectionType))
                .ToList();
        }


        public List<DungeonTileConnection> GetDoorsForAreaAndType(DungeonArea area,
            List<TileConnectionType> allowedTypes)
        {
            return tileDoors
                .Where(door => door.dungeonArea.Equals(area) && allowedTypes.Contains(door.tileConnectionType))
                .ToList();
        }

        public void SetOccupiedByPlayer()
        {
            _tileIsOccupiedByPlayer = true;
        }

        public void DestroyTile()
        {
            Destroy(gameObject);
        }

        public void DeactivateTile()
        {
            gameObject.SetActive(false);
        }

        public void ReactivateTile()
        {
            gameObject.SetActive(true);
        }
    }
}