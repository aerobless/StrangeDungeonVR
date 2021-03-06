using HurricaneVR.Framework.Core;
using SixtyMeters.logic.item;
using UnityEngine;

namespace SixtyMeters.logic.variability.effects
{
    public class SoulShardHelper : PlayerItem
    {
        // Components
        public AudioSource audioSource;
        public AudioClip soulShardApplied;
        public GameObject fx;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ApplySoulShard()
        {
            fx.SetActive(false);
            audioSource.Stop();
            audioSource.PlayOneShot(soulShardApplied);
        }
    }
}