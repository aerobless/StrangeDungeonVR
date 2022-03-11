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
            return tileDoors.Where(door => door.IsAttached())
                .Select(door => door.GetAttachedTile())
                .ToList();
        }

        public void NotifyPlayerEnterOrExit()
        {
            _tileIsOccupiedByPlayer = !_tileIsOccupiedByPlayer;
            if (_tileIsOccupiedByPlayer)
            {
                Debug.Log("Player has entered tile " + gameObject.name);
                _dungeonGenerator.GenerateNewTilesFor(this);
                _dungeonGenerator.RemoveExpiredTiles(this);

                // Statistics
                _statistics.StartTrackingIfNotStartedYet();
                ++_statistics.roomsDiscovered;
            }
            else
            {
                Debug.Log("Player has left tile " + gameObject.name);
            }
        }

        public List<DungeonTileConnection> GetAttachedDoors()
        {
            return tileDoors.Where(door => door.IsAttached())
                .Select(door => door.GetAttachedDoor())
                .ToList();
        }

        public void SetOccupiedByPlayer()
        {
            _tileIsOccupiedByPlayer = true;
        }

        public bool HasLockedDoors()
        {
            return tileDoors.Any(door => door.IsLocked());
        }

        public void Remove(float destructionTime = 1f)
        {
            GetAttachedDoors().ForEach(door => door.Lock());
            Destroy(gameObject, destructionTime);
        }
    }
}