using System;
using System.Collections.Generic;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SixtyMeters.logic.variability.effects
{
    public class RandomEffect : MonoBehaviour, IRandomizeable
    {
        public List<VariabilityEffect> possibleEffects;

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
            var chosenEffect = Helper.GETRandomFromList(possibleEffects);
            gameObject.AddComponent(chosenEffect.GetType());
            Debug.Log("Chosen type" + chosenEffect.GetType());
            Destroy(this);
        }
    }
}