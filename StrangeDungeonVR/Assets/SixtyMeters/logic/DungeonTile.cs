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
        public bool isCenter;

        // Start is called before the first frame update
        void Start()
        {
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
    }
}
