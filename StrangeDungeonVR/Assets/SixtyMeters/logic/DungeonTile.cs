using System.Collections.Generic;
using SixtyMeters.logic.door;
using UnityEngine;

namespace SixtyMeters.logic
{
    public class DungeonTile : MonoBehaviour
    {
        public List<DungeonTileConnection> tileDoors;

        //The current center of the game, moves with the player
        public bool isCenter;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
