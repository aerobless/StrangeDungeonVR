using System.Collections.Generic;
using System.Linq;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.generator.randomization
{
    public class DecorationSelector : MonoBehaviour, IRandomizeable
    {
        public List<GameObject> decorationOptions;

        void Start()
        {
            if (decorationOptions.Count == 0)
            {
                Debug.LogError(name + " has no decoration options defined!");
            }
        }

        public void Randomize()
        {
            decorationOptions.ForEach(decoration => decoration.SetActive(false));
            var randomDecoration = Helper.GETRandomFromList(decorationOptions);
            randomDecoration.SetActive(true);
            RandomizeSubDecorations(randomDecoration);
            Debug.Log("Randomized decorations, chose "+randomDecoration.name);
        }

        private static void RandomizeSubDecorations(GameObject parentObject)
        {
            var subDecorations = parentObject.GetComponentsInChildren<IRandomizeable>();
            subDecorations.ToList().ForEach(decoration => decoration.Randomize());
        }
    }
}