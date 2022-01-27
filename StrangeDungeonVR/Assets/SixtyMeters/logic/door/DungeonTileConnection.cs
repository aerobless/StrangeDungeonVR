using UnityEngine;

namespace SixtyMeters.logic.door
{
    public class DungeonTileConnection : MonoBehaviour
    {
        // Used to find compatible tile connections
        public string connectionId;
        
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
        
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(Vector3.zero,Vector3.one);
            Gizmos.DrawLine(Vector3.zero,Vector3.forward);

            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero+new Vector3(0,0,-3f), new Vector3(0.1f, 0.1f, 3f));
        }
    }
}