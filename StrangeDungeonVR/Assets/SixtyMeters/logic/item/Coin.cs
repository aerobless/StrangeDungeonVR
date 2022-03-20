using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.logic.item
{
    public class Coin : PlayerItem
    {
        public AudioSource playerAudioSource;
        public AudioClip coinDepositSound;

        public bool isCollectible = false;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(EnableCoinCollection());
        }

        // Update is called once per frame
        void Update()
        {
        }

        private IEnumerator EnableCoinCollection()
        {
            yield return new WaitForSeconds(2);
            isCollectible = true;
        }

        public void Deposit()
        {
            isCollectible = false;
            playerAudioSource.PlayOneShot(coinDepositSound);
            Destroy(gameObject, 0.97f);
        }
    }
}