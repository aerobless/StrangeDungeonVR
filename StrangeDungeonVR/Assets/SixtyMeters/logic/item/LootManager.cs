using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.item
{
    public class LootManager : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnableLoot
        {
            [Tooltip("The item to be spawned as reward.")]
            public GameObject lootItem;

            [Tooltip("If the item is spawned (after chance), this determines how many of the item are spawned.")]
            public int minAmount;

            [Tooltip("If the item is spawned (after chance), this determines how many of the item are spawned.")]
            public int maxAmount;

            [Tooltip("Chance of the loot spawning in percentage (1-100). 100 means it's always spawning.")]
            public int chance;
        }

        [System.Serializable]
        public class LootBundle
        {
            public string lootBundleId;
            public List<SpawnableLoot> loot = new();
        }

        public List<LootBundle> lootBundles = new();

        private LootBundle GetLootBundle(string bundleId)
        {
            var lootBundle = lootBundles.Find(bundle => bundle.lootBundleId.ToLower().Equals(bundleId.ToLower()));
            if (lootBundle == null)
            {
                Debug.LogError("No loot bundle with name " + bundleId + " found!");
            }

            return lootBundle;
        }

        public List<GameObject> GetPackagedLoot(string bundleId)
        {
            var lootBundle = GetLootBundle(bundleId);
            List<GameObject> packagedLoot = new();
            lootBundle.loot.ForEach(lootItem =>
            {
                // Calculate chance (1-100)
                var luckyRoll = Random.Range(1, 100) <= lootItem.chance;
                if (luckyRoll)
                {
                    // Calculate how many of the item are spawned
                    var amount = Random.Range(lootItem.minAmount, lootItem.maxAmount);
                    for (int i = 0; i < amount; i++)
                    {
                        packagedLoot.Add(lootItem.lootItem);
                    }
                }
            });

            return packagedLoot;
        }


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