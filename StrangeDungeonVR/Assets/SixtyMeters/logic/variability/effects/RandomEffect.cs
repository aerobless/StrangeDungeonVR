using System.Collections.Generic;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.variability.effects
{
    public class RandomEffect : MonoBehaviour, IRandomizeable
    {
        public List<VariabilityEffect> possibleEffects;

        // Start is called before the first frame update
        void Start()
        {
            Randomize();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Randomize()
        {
            var chosenEffect = Helper.GETRandomFromList(possibleEffects);
            gameObject.AddComponent(chosenEffect.GetType());
            Destroy(this);
        }
    }
}