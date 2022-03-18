using System.Collections.Generic;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.generator.randomization
{
    public class RandomGameObjectV2 : MonoBehaviour, IRandomizeable
    {
        [System.Serializable]
        public class Possibility : IChanceTicket
        {
            public GameObject gameObject;

            [Tooltip("The more tickets an object has the more likely it is to win and be spawned.")]
            public int chanceTickets = 1;

            public int GetTicketCount()
            {
                return chanceTickets;
            }
        }

        // Replacement objects that will be used instead of the existing one, should match in size
        public List<Possibility> possibilities;

        // If defined, the chosen possibility will be applied to each GO in the replacement set
        // this is useful when a room should have the same random object in multiple places
        // if not defined the possibility will only be applied to this GO itself
        public List<GameObject> replacementSet;
        
        [Tooltip("Between 1 - 100. If 100 it the object is always there, if 1 it has a 1 in 10 chance to appear")]
        public int appearanceChance = 100;

        public void Randomize()
        {
            var randomFromList = Helper.GETRandomFromList(possibilities).gameObject;

            if (replacementSet.Count > 0)
            {
                ReplaceGameObjectsInSet(randomFromList);
            }
            

            if (RollObjectAppearance())
            {
                InstantiateWithOriginalParent(randomFromList, transform);
            }

            Destroy(gameObject);
        }

        private void InstantiateWithOriginalParent(GameObject randomFromList, Transform currentTransform)
        {
            var instantiatedObj = Instantiate(randomFromList, currentTransform.position, currentTransform.rotation);
            instantiatedObj.transform.parent = transform.parent;
        }

        private bool RollObjectAppearance()
        {
            var random = Random.Range(1, 100);
            return appearanceChance == 10 || random < appearanceChance;
        }

        private void ReplaceGameObjectsInSet(GameObject randomFromList)
        {
            foreach (var go in replacementSet)
            {
                InstantiateWithOriginalParent(randomFromList, go.transform);
                Destroy(go);
            }
        }

        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position

            Gizmos.color = new Color(0, 0, 1, 0.3f);
            if (replacementSet.Count > 0)
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}