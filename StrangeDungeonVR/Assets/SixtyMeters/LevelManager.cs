using UnityEngine;

namespace SixtyMeters
{
    public class LevelManager : MonoBehaviour
    {
        public bool randomizeSeed;
        public int seed;

        // Start is called before the first frame update
        void Start()
        {
            PrepareSeed();
        }

        private void PrepareSeed()
        {
            if (randomizeSeed)
            {
                seed = (int) System.DateTime.Now.Ticks;
            }
            Random.InitState(seed);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}