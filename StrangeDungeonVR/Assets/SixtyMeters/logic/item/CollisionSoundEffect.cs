using System.Collections.Generic;
using SixtyMeters.logic.sound;
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

        [Tooltip("Play only the passive sound effect from the impacting object. " +
                 "Automatically enabled if no audioClips are provided.")]
        public bool playOnlyPassiveSound;

        [Tooltip("Play only the active sound of this object and ignore passive sound information.")]
        public bool playOnlyActiveSound;

        [Tooltip("The type of object/material that is making the impact. E.g. a sword")]
        public CollisionInputVariant collisionInputMaterial = CollisionInputVariant.Any;

        // Internals
        private CollisionSoundManager _collisionSoundManager;

        // Start is called before the first frame update
        void Start()
        {
            if (audioClip)
            {
                audioClips.Add(audioClip);
            }

            if (audioClips.Count == 0)
            {
                playOnlyPassiveSound = true;
            }

            _collisionSoundManager = FindObjectOfType<GameManager>().collisionSoundManager;
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
                if (!playOnlyPassiveSound)
                {
                    audioSource.PlayOneShot(Helper.GETRandomFromList(audioClips), audioLevel);
                }

                var collisionInfo = collision.gameObject.GetComponent<PassiveCollisionInformation>();
                if (!playOnlyActiveSound && collisionInfo)
                {
                    var collisionAudioClips =
                        _collisionSoundManager.GetAudioClipsForMaterial(collisionInfo.material, collisionInputMaterial);
                    if (collisionAudioClips.Count > 0)
                    {
                        audioSource.PlayOneShot(Helper.GETRandomFromList(collisionAudioClips), audioLevel);
                    }
                }
            }
        }
    }
}