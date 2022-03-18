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
        public GameObject chestKey;
        public AudioSource audioSource;

        public List<AudioClip> unlockSounds;
        public List<AudioClip> chestOpenSounds;

        public GameObject lootEffect;

        // Internals
        private LootSpawner _spawner;
        private bool _unlocked = false;

        // Start is called before the first frame update
        void Start()
        {
            _spawner = GetComponent<LootSpawner>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            var key = other.GetComponent<Key>();
            if (key && !_unlocked)
            {
                _unlocked = true;
                Destroy(key.gameObject);
                chestKey.SetActive(true);
                StartCoroutine(Helper.LerpRotation(chestKey.transform, Quaternion.Euler(-90, 90, 90), 0.5f,
                    () => { chestKey.SetActive(false); }));
                StartCoroutine(Helper.PlaySound(audioSource, unlockSounds, 0.5f, () =>
                {
                    audioSource.PlayOneShot(Helper.GETRandomFromList(chestOpenSounds));
                    var targetRotation = Quaternion.Euler(-120, 0, 0);
                    StartCoroutine(Helper.Wait(0.5f, () =>
                    {
                        _spawner.Spawn();
                        lootEffect.SetActive(true);
                    }));
                    StartCoroutine(Helper.LerpRotation(chestLid.transform, targetRotation, 1.7f,
                        () => { }));
                }));
            }
        }
    }
}