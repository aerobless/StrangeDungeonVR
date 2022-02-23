using SixtyMeters.logic.interfaces;
using UnityEngine;

namespace SixtyMeters.logic.generator.special
{
    public class DeathTile : MonoBehaviour, IHasSpawnPoint
    {
        public WayPoint playerSpawnLocation;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }


        public WayPoint GetSpawnPoint()
        {
            return playerSpawnLocation;
        }
    }
}