using System;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.player;
using SixtyMeters.logic.utilities;
using UnityEditor;
using UnityEngine;

namespace SixtyMeters.logic.traps
{
    public class SpikeTrap : MonoBehaviour
    {
        public GameObject spikes;
        public Vector3 loweredPosition;
        public Vector3 fullyExtendedPosition;
        public float spikeExtensionTime = 0.07f;
        public float spikeLoweringTime = 2f;
        public float spikeWaitBeforeLoweringTime = 3f;
        public float damagePerHit = 50;

        public AudioSource audioSource;
        public AudioClip spikesExtending;
        public AudioClip spikesRetracting;

        // Used to prevent the trap from being triggered again before its fully reset
        private bool _triggered;

        // Start is called before the first frame update
        void Start()
        {
            spikes.transform.localPosition = loweredPosition;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void TriggerTrap(Collider coll)
        {
            if (!_triggered && coll.GetComponent<TriggerCollider>())
            {
                _triggered = true;
                RaiseSpikes(Wait(LowerSpikes(ResettingTrapFinished())));

                coll.GetComponentInParent<IDamageable>()?.ApplyDamage(damagePerHit);
            }
        }

        private void RaiseSpikes(Action doAfter)
        {
            audioSource.PlayOneShot(spikesExtending);
            StartCoroutine(Helper.LerpPosition(spikes.transform, fullyExtendedPosition, spikeExtensionTime, doAfter));
        }

        private Action Wait(Action doAfter)
        {
            return () => { StartCoroutine(Helper.Wait(spikeWaitBeforeLoweringTime, doAfter)); };
        }

        private Action LowerSpikes(Action doAfter)
        {
            return () =>
            {
                audioSource.PlayOneShot(spikesRetracting);
                StartCoroutine(Helper.LerpPosition(spikes.transform, loweredPosition, spikeLoweringTime, doAfter));
            };
        }

        private Action ResettingTrapFinished()
        {
            return () => { _triggered = false; };
        }
    }
}