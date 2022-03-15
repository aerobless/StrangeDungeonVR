using System.Collections.Generic;
using SixtyMeters.logic.item;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.props
{
    public class LockedChest : MonoBehaviour
    {
        // Components
        public GameObject chestLid;
        public AudioSource audioSource;

        public List<AudioClip> unlockSounds;
        public List<AudioClip> chestOpenSounds;

        public ParticleSystem lootEffect;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            var key = other.GetComponent<Key>();
            if (key)
            {
                Destroy(key.gameObject);
                StartCoroutine(Helper.PlaySound(audioSource, unlockSounds, () =>
                {
                    audioSource.PlayOneShot(Helper.GETRandomFromList(chestOpenSounds));
                    var targetRotation = Quaternion.Euler(-120, 0, 0);
                    StartCoroutine(Helper.LerpRotation(chestLid.transform, targetRotation, 1.7f,
                        () => { StartCoroutine(Helper.PlayParticles(lootEffect, 1f, () => { })); }));
                }));
            }
        }
    }
}