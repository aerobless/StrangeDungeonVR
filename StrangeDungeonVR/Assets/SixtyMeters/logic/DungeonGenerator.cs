using System.Collections.Generic;
using System.Linq;
using SixtyMeters.logic.door;
using SixtyMeters.utilities;
using UnityEngine;

namespace SixtyMeters.logic
{
    public class DungeonGenerator : MonoBehaviour
    {
        public List<GameObject> tiles;
        public DungeonTile startTile;

        // Start is called before the first frame update
        void Start()
        {
            var tileDoorOnStartTile = startTile.tileDoors[0];

            //Spawn new Tile
            var randomNextTile = Helper.GETRandomFromList(tiles);
            var newTile = Instantiate(randomNextTile, Vector3.zero, Quaternion.identity);
            //Align new tile with existing tile
            AlignAndAttachTileDoor(tileDoorOnStartTile, newTile);
            tileDoorOnStartTile.Attach();
            //Iterate over doors on new tile and a) add tile or b) close them off

            // Initial
            var createdTiles = CreateTilesForAttachmentPoints(newTile.GetComponent<DungeonTile>());
            
            // Recursion Loop 1
            foreach (var createdTile in createdTiles)
            {
                var createdTiles2 = CreateTilesForAttachmentPoints(createdTile);
                
                // Recursion Loop 2
                foreach (var createdTile2 in createdTiles2)
                {
                    CreateTilesForAttachmentPoints(createdTile2);
                }
            }
        }

        private List<DungeonTile> CreateTilesForAttachmentPoints(DungeonTile newTile)
        {
            var createdTiles = new List<DungeonTile>();
            newTile.tileDoors
                .Where(door => !door.IsAttached())
                .ToList().ForEach(door => AttachNewTile(door, createdTiles));
            return createdTiles;
        }

        private void AttachNewTile(DungeonTileConnection dungeonTileConnection, List<DungeonTile> createdTiles)
        {
            var newTile2 = Instantiate(tiles[0], Vector3.zero, Quaternion.identity);
            AlignAndAttachTileDoor(dungeonTileConnection, newTile2);
            dungeonTileConnection.Attach();
            createdTiles.Add(newTile2.GetComponent<DungeonTile>());
        }

        /// <summary>
        /// Aligns a new tile on an existing door.
        /// </summary>
        /// <param name="existingConnection">the door in an existing tile, will not be moved</param>
        /// <param name="newTile">the new tile, will be moved to attach to the existing door</param>
        private void AlignAndAttachTileDoor(DungeonTileConnection existingConnection, GameObject newTile)
        {
            var randomDoorInNewTile = Helper.GETRandomFromList(newTile.GetComponent<DungeonTile>().tileDoors);
            randomDoorInNewTile.Attach();

            var targetTransform = existingConnection.transform;
            var newTileParentTransform = newTile.transform;
            var newTileChildDoorTransform = randomDoorInNewTile.transform;

            var childRotToTarget = targetTransform.rotation * Quaternion.Inverse(newTileChildDoorTransform.rotation);
            newTileParentTransform.rotation = childRotToTarget;
            ReverseTile(newTileParentTransform);

            var childToTarget = targetTransform.position - newTileChildDoorTransform.position;
            newTileParentTransform.position += childToTarget;
        }

        private void ReverseTile(Transform tile)
        {
            tile.transform.RotateAround(tile.transform.position, transform.up, 180f);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}