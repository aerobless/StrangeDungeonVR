using UnityEngine;

namespace SixtyMeters.logic.generator
{
    public class WayPoint : MonoBehaviour
    {
        public bool hasDirection = false;
        //public WayPointDestination destination;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
        
        public Vector3 GetLocalPosition()
        {
            return transform.localPosition;
        }

        void OnDrawGizmos()
        {
            if (hasDirection)
            {
                // Location indicator
                Gizmos.color = Color.yellow;
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(Vector3.zero, new Vector3(0.2f, 0.2f, 0.2f));

                // Direction indicator
                Gizmos.color = Color.red;
                Gizmos.DrawCube(Vector3.zero + new Vector3(0, 0, 0.1f), new Vector3(0.05f, 0.05f, 0.05f));
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
            }
        }
    }
}