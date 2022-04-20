using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.traps
{
    public class FireSpitTrap : MonoBehaviour, ITrap
    {
        // Components
        public List<ParticleSystem> fireParticles;

        public AudioSource audioSource;
        public List<AudioClip> fireBreathSound;

        // Settings
        public float fireActiveTime;

        // Internals
        private ParticleSystem _chosenFireEffect;
        private bool _triggered;

        // Start is called before the first frame update
        void Start()
        {
            _chosenFireEffect = Helper.GETRandomFromList(fireParticles);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void TriggerTrap()
        {
            if (!_triggered)
            {
                // Wait before spitting, make sound

                // Spit + sound effect
                _triggered = true;
                _chosenFireEffect.gameObject.SetActive(true);
                audioSource.PlayOneShot(Helper.GETRandomFromList(fireBreathSound));
                _chosenFireEffect.Play();

                // Reset
                StartCoroutine(Helper.Wait(fireActiveTime, ResetTrap));
            }
        }

        public void ResetTrap()
        {
            _triggered = false;
            _chosenFireEffect.Stop();
            _chosenFireEffect.gameObject.SetActive(false);
        }
    }
}