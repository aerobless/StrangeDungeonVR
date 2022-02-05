using System;
using System.Collections.Generic;
using System.Linq;
using SixtyMeters.logic.door;
using SixtyMeters.utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SixtyMeters.logic
{
    public class DungeonGenerator : MonoBehaviour
    {
        public List<GameObject> tiles;
        public DungeonTile startTile;

        // Start is called before the first frame update
        void Start()
        {
            // Generates the first tile connecting to the starting tile that is already in the world
            GenerateNewTilesFor(startTile);
        }

        public void GenerateNewTilesFor(DungeonTile existingTile)
        {
            existingTile.tileDoors
                .Where(door => !door.IsAttached())
                .ToList().ForEach(doorInExistingTile => AttachNewTile(doorInExistingTile));
        }

        private List<DungeonTile> AttachNewTile(DungeonTileConnection doorInExistingTile)
        {
            var createdTiles = new List<DungeonTile>();
            var newTile = SpawnRandomNewTile();
            AlignAndAttachTileDoor(doorInExistingTile, newTile);
            createdTiles.Add(newTile.GetComponent<DungeonTile>());
            return createdTiles;
        }

        private GameObject SpawnRandomNewTile()
        {
            var randomNextTile = Helper.GETRandomFromList(tiles);
            var newTile = Instantiate(randomNextTile, Vector3.zero, Quaternion.identity);

            // The tile seed is intended to avoid player decisions (go left or right) messing with the predictable
            // nature of a seeded level. By setting a seed for each door connection point it does not matter what
            // the player decides, the seed will be based on the original random seed
            newTile.GetComponent<DungeonTile>().tileSeed = Random.Range(0, Int32.MaxValue);
            return newTile;
        }

        /// <summary>
        /// Aligns a new tile on an existing door.
        /// </summary>
        /// <param name="doorInExistingTile">the door in an existing tile, will not be moved</param>
        /// <param name="newTile">the new tile, will be moved to attach to the existing door</param>
        private void AlignAndAttachTileDoor(DungeonTileConnection doorInExistingTile, GameObject newTile)
        {
            var randomDoorInNewTile = Helper.GETRandomFromList(newTile.GetComponent<DungeonTile>().tileDoors);
            randomDoorInNewTile.Attach(doorInExistingTile);
            doorInExistingTile.Attach(randomDoorInNewTile);

            var targetTransform = doorInExistingTile.transform;
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