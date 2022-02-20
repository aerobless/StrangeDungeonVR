using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.item
{
    public class LootSpawner : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnableLoot
        {
            public GameObject loot;
            public int minAmount;
            public int maxAmount;
        }

        public bool spawnAtStart;

        public List<SpawnableLoot> loot = new();

        // Start is called before the first frame update
        void Start()
        {
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
            var randomLoot = Helper.GETRandomFromList(loot);
            var amount = Random.Range(randomLoot.minAmount, randomLoot.minAmount);

            for (int i = 0; i < amount; i++)
            {
                var randomX = Random.Range(-0.2f, 0.2f);
                var randomZ = Random.Range(-0.2f, 0.2f);
                var spawnLocation = gameObject.transform.position + new Vector3(randomX, 0.5f, randomZ);
                var coin = Instantiate(randomLoot.loot, spawnLocation, Quaternion.identity);
                coin.transform.parent = gameObject.transform;
            }
        }
    }
}