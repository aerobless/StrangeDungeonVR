using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.sound
{
    public class AmbientSoundOffset : MonoBehaviour
    {
        public AudioSource audioSource;
        public List<AudioClip> ambientSounds;
        public int maxRandomDelayInSeconds = 2;

        // Start is called before the first frame update
        void Start()
        {
            PlayRandomAmbientSound();
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: switch between multiple random audio clips
        }

        private void PlayRandomAmbientSound()
        {
            var delay = Random.Range(0, maxRandomDelayInSeconds);
            var chosenSound = Helper.GETRandomFromList(ambientSounds);
            audioSource.clip = chosenSound;
            audioSource.PlayDelayed(delay);
        }
    }
}