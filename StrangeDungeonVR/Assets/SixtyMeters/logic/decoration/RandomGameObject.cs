using System.Collections.Generic;
using SixtyMeters.utilities;
using UnityEngine;

namespace SixtyMeters.logic.decoration
{
    public class RandomGameObject : MonoBehaviour
    {
        // Replacement objects that will be used instead of the existing one, should match in size
        public List<GameObject> possibilities;

        // If defined, the chosen possibility will be applied to each GO in the replacement set
        // this is useful when a room should have the same random object in multiple places
        // if not defined the possibility will only be applied to this GO itself
        public List<GameObject> replacementSet;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Randomize()
        {
            var randomFromList = Helper.GETRandomFromList(possibilities);

            if (replacementSet.Count > 0)
            {
                ReplaceGameObjectsInSet(randomFromList);
            }

            var currentTransform = transform;
            Instantiate(randomFromList, currentTransform.position, currentTransform.rotation);
            Destroy(gameObject);
        }

        private void ReplaceGameObjectsInSet(GameObject randomFromList)
        {
            foreach (var go in replacementSet)
            {
                var currentTransform = go.transform;
                Instantiate(randomFromList, currentTransform.position, currentTransform.rotation);
                Destroy(go);
            }
        }

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position

            Gizmos.color = Color.blue;
            if (replacementSet.Count > 0)
            {
                Gizmos.color = Color.green;   
            }
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}