using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.logic.ai.appearance
{
    public class UniversalAgentAppearance : MonoBehaviour
    {
        [System.Serializable]
        public class SkinConfiguration
        {
            public Skin skinId;
            public GameObject skinObject;
        }

        public List<SkinConfiguration> skinConfigurations;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public GameObject SetAppearance(Skin skin)
        {
            GameObject chosenAppearance = null;
            skinConfigurations.ForEach(skinConfig =>
            {
                skinConfig.skinObject.SetActive(skinConfig.skinId.Equals(skin));
                if (skinConfig.skinId.Equals(skin))
                {
                    chosenAppearance = skinConfig.skinObject;
                }
            });
            return chosenAppearance;
        }
    }
}