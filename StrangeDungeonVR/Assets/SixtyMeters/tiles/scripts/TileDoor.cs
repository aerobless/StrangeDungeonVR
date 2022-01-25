using UnityEngine;

namespace SixtyMeters.tiles.scripts
{
    public class TileDoor : MonoBehaviour
    {
        private bool _isAttached;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public bool IsAttached()
        {
            return _isAttached;
        }

        public void Attach()
        {
            _isAttached = true;
        }
    }
}