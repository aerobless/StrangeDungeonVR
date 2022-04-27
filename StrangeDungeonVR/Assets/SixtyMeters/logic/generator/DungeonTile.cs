using System.Collections.Generic;
using System.Linq;
using SixtyMeters.logic.analytics;
using SixtyMeters.logic.door;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
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
        protected GameManager GameManager;

        // Start is called before the first frame update
        void Start()
        {
            GameManager = GameManager.Instance;
            //_dungeonGenerator = FindObjectOfType<DungeonGenerator>(); //TODO: replace
            //_statistics = FindObjectOfType<StatisticsManager>();

            var randomizedElements = GetComponentsInChildren<IRandomizeable>();
            foreach (var go in randomizedElements)
            {
                go.Randomize();
            }

            LateInit();
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
                GameManager.dungeonGenerator.TileEntered(this);

                // Statistics
                GameManager.statisticsManager.StartTrackingIfNotStartedYet();
                ++GameManager.statisticsManager.roomsDiscovered;
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

        protected virtual void LateInit()
        {
            // do nothing, used to be overwritten by children
        }
    }
}