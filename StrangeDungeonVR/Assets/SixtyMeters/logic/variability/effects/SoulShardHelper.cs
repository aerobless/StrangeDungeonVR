using HurricaneVR.Framework.Core;
using UnityEngine;

namespace SixtyMeters.logic.variability.effects
{
    public class SoulShardHelper : MonoBehaviour
    {
        // Components
        public AudioSource audioSource;
        public AudioClip soulShardApplied;

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
            audioSource.Stop();
            audioSource.PlayOneShot(soulShardApplied);
        }
    }
}