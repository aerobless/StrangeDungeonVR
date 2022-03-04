using System;
using System.Collections.Generic;
using System.Linq;
using SixtyMeters.logic.door;
using SixtyMeters.logic.generator.special;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SixtyMeters.logic.generator
{
    public class DungeonGenerator : MonoBehaviour
    {
        public List<GameObject> tiles;
        public DungeonTile startTile;
        public DungeonTile restartTile;
        public DungeonTile deathTile;

        private DungeonTile _currentCenterTile;

        private readonly List<DungeonTile> _activeTiles = new();

        // Start is called before the first frame update
        void Start()
        {
            // The player starts off in this tile, so we need to set it as occupied manually
            startTile.SetOccupiedByPlayer();
            _currentCenterTile = startTile;

            // Generates the first tile connecting to the starting tile that is already in the world
            GenerateNewTilesFor(startTile);
        }

        public void GenerateNewTilesFor(DungeonTile existingTile)
        {
            _currentCenterTile = existingTile;
            existingTile.tileDoors
                .Where(door => door.IsUnattached())
                .ToList().ForEach(doorInExistingTile => AttachNewTile(doorInExistingTile));
        }

        public void RemoveExpiredTiles(DungeonTile existingTile)
        {
            if (!existingTile.HasLockedDoors())
            {
                // Only delete when entering a fresh corridor
                existingTile.GetAttachedTiles()
                    .SelectMany(tile => tile.GetAttachedTiles())
                    .Distinct()
                    .Where(tile => tile != existingTile)
                    .ToList().ForEach(tileToBeRemoved =>
                    {
                        _activeTiles.Remove(tileToBeRemoved);
                        tileToBeRemoved.Remove();
                    });
            }
        }

        public StartTile GenerateRespawnTile(Transform playerTransform)
        {
            // Setup respawn tile
            var respawnTile = Instantiate(restartTile, playerTransform.transform.position, Quaternion.identity);
            AlignTileToPlayer(playerTransform, respawnTile.gameObject);

            // Destroy all remaining dungeon tiles & add this one to the list
            _activeTiles.ForEach(tile => tile.Remove(0f));
            _activeTiles.Clear();
            _activeTiles.Add(respawnTile);

            // Prepare for next run
            _currentCenterTile = respawnTile;
            respawnTile.SetOccupiedByPlayer();
            GenerateNewTilesFor(respawnTile);
            return respawnTile.GetComponent<StartTile>();
        }

        private List<DungeonTile> AttachNewTile(DungeonTileConnection doorInExistingTile)
        {
            var createdTiles = new List<DungeonTile>();
            var newTile = SpawnRandomNewTile();
            AlignAndAttachTileDoor(doorInExistingTile, newTile);
            createdTiles.Add(newTile.GetComponent<DungeonTile>());
            _activeTiles.AddRange(createdTiles);
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

        private void AlignTileToPlayer(Transform playerTransform, GameObject newTile)
        {
            var randomDoorInNewTile = newTile.GetComponent<IHasSpawnPoint>().GetSpawnPoint().transform;

            var targetTransform = playerTransform;
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