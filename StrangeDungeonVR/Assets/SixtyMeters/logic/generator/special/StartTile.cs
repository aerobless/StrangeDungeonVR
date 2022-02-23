using SixtyMeters.logic.interfaces;
using UnityEngine;

namespace SixtyMeters.logic.generator.special
{
    public class StartTile : MonoBehaviour, IHasSpawnPoint
    {
        public WayPoint playerSpawnLocation;
        public GameObject graveyard;

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

        public void EnableGraveyard()
        {
            graveyard.SetActive(true);
        }
    }
}