using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SixtyMeters.logic.item
{
    public class LootSpawner : MonoBehaviour
    {
        [Tooltip("Whether the loot should be spawned right away. Useful for broken items.")]
        public bool spawnAtStart;

        [Tooltip("The id of the lootBundle that should be spawned. Bundles are defined in the LootManager.")]
        public string lootBundleId;

        [Tooltip("The height at which the items should be spawned")]
        public float spawnHeight;

        // Internals
        private GameManager _gameManager;
        private List<PlayerItem> _packagedLoot;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _packagedLoot = _gameManager.lootManager.GetPackagedLoot(lootBundleId);
            if (spawnAtStart)
            {
                Spawn();
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Spawn()
        {
            _packagedLoot.ForEach(lootItem =>
            {
                var randomX = Random.Range(-0.2f, 0.2f);
                var randomZ = Random.Range(-0.2f, 0.2f);
                var spawnLocation = gameObject.transform.position + new Vector3(randomX, spawnHeight, randomZ);
                var spawnedItem = Instantiate(lootItem.gameObject, spawnLocation, Quaternion.identity);

                SetParentBasedOnPersistenceSetting(lootItem, spawnedItem);
            });
        }

        private void SetParentBasedOnPersistenceSetting(PlayerItem lootItem, GameObject spawnedItem)
        {
            switch (lootItem.itemPersistence)
            {
                case ItemPersistence.None:
                    spawnedItem.transform.parent = gameObject.transform;
                    break;
                case ItemPersistence.Room:
                    spawnedItem.transform.parent = _gameManager.dungeonGenerator.GetCurrentCenterTile().transform;
                    break;
                case ItemPersistence.Persistent:
                    spawnedItem.transform.parent = _gameManager.lootManager.transform;
                    break;
                default:
                    Debug.LogError("No switch case for " + lootItem.itemPersistence);
                    break;
            }
        }
    }
}