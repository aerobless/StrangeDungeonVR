using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.item
{
    public class CollisionSoundEffect : MonoBehaviour
    {
        [Tooltip("The audio source from which the sound should be emitted.")]
        public AudioSource audioSource;

        [Tooltip("If only a single clip is set it will be added to the list of audioclips.")]
        public AudioClip audioClip;

        [Tooltip("A list of audio clips to be played on impact. A random clip is chosen.")]
        public List<AudioClip> audioClips;

        // Start is called before the first frame update
        void Start()
        {
            if (audioClip)
            {
                audioClips.Add(audioClip);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > 0.2)
            {
                float audioLevel = collision.relativeVelocity.magnitude / 10.0f;
                audioSource.PlayOneShot(Helper.GETRandomFromList(audioClips), audioLevel);
            }
        }
    }
}