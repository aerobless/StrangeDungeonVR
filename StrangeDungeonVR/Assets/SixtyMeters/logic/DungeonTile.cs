using System.Collections.Generic;
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

        public void NotifyPlayerEnterOrExit()
        {
            _tileIsOccupiedByPlayer = !_tileIsOccupiedByPlayer;
            if (_tileIsOccupiedByPlayer)
            {
                Debug.Log("Player has entered tile " + gameObject.name);
                _dungeonGenerator.GenerateNewTilesFor(this);
            }
            else
            {
                Debug.Log("Player has left tile " + gameObject.name);
            }
        }
    }
}