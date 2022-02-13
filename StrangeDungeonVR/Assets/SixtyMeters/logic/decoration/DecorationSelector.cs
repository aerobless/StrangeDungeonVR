using System.Collections.Generic;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.decoration
{
    public class DecorationSelector : MonoBehaviour, IRandomizeable
    {
        public List<GameObject> decorationOptions;

        // Start is called before the first frame update
        void Start()
        {
            if (decorationOptions.Count == 0)
            {
                Debug.LogError(name + " has no decoration options defined!");
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Randomize()
        {
            decorationOptions.ForEach(option => option.SetActive(false));
            Helper.GETRandomFromList(decorationOptions).SetActive(true);
        }
    }
}