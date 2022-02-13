using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.audio
{
    public class PlayRandomSoundEffect : MonoBehaviour
    {
        public List<AudioClip> soundEffects;
        public AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource.PlayOneShot(Helper.GETRandomFromList(soundEffects));
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}