using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.item
{
    public class LootSpawner : MonoBehaviour
    {
        [Tooltip("Whether the loot should be spawned right away. Useful for broken items.")]
        public bool spawnAtStart;

        [Tooltip("The id of the lootBundle that should be spawned. Bundles are defined in the LootManager.")]
        public string lootBundleId;

        // Internals
        private LootManager _lootManager;
        private List<GameObject> _packagedLoot;

        // Start is called before the first frame update
        void Start()
        {
            _lootManager = FindObjectOfType<LootManager>();
            _packagedLoot = _lootManager.GetPackagedLoot(lootBundleId);
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
                var spawnLocation = gameObject.transform.position + new Vector3(randomX, 0.5f, randomZ);
                var spawnedItem = Instantiate(lootItem, spawnLocation, Quaternion.identity);
                spawnedItem.transform.parent = gameObject.transform;
            });
        }
    }
}