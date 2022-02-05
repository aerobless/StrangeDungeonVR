using System.Collections.Generic;
using System.Linq;
using SixtyMeters.logic.decoration;
using SixtyMeters.logic.door;
using UnityEngine;

namespace SixtyMeters.logic
{
    public class DungeonTile : MonoBehaviour
    {
        public List<DungeonTileConnection> tileDoors;

        // Set when this tile is instantiated by the generator
        public int tileSeed;

        // The current center of the game, moves with the player
        private bool _tileIsOccupiedByPlayer = false;

        private DungeonGenerator _dungeonGenerator;

        // Start is called before the first frame update
        void Start()
        {
            _dungeonGenerator = FindObjectOfType<DungeonGenerator>();

            var randomizedElements = GetComponentsInChildren<RandomGameObject>();
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

        public void Remove()
        {
            GetAttachedDoors().ForEach(door => door.Lock());
            Destroy(gameObject, 1f);
        }
    }
}