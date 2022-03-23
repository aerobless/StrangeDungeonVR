using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.logic.sound
{
    public class CollisionSoundManager : MonoBehaviour
    {
        public List<MaterialSoundConfiguration> audioLibrary;

        [System.Serializable]
        public class MaterialSoundConfiguration
        {
            public CollisionMaterialType materialType;
            public List<CollisionVariant> collisionVariants;
        }

        [System.Serializable]
        public class CollisionVariant
        {
            public CollisionInputVariant inputVariant;
            public List<AudioClip> audioClips;
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public List<AudioClip> GetAudioClipsForMaterial(CollisionMaterialType materialType,
            CollisionInputVariant inputVariant)
        {
            var soundConfig = audioLibrary.Find(entry => entry.materialType.Equals(materialType));
            if (soundConfig != null)
            {
                var collisionVariantConfig =
                    soundConfig.collisionVariants.Find(entry => entry.inputVariant.Equals(inputVariant)) ??
                    soundConfig.collisionVariants.Find(entry => entry.inputVariant.Equals(CollisionInputVariant.Any));

                if (collisionVariantConfig != null)
                {
                    return collisionVariantConfig.audioClips;
                }
            }

            return new List<AudioClip>();
        }
    }
}