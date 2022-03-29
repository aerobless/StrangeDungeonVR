using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.traps
{
    public class FallingSteppingStone : MonoBehaviour, ITrap
    {
        // Components
        public AudioSource audioSource;
        public List<AudioClip> warnClips;
        public List<AudioClip> fallClips;

        // Settings
        public float onStepFallDistance;
        public float onStepFallTime;

        public float gracePeriodUntilFall;
        public float deathFallDistance;
        public float deathFallTime;

        public float timeBeforeReset;
        public float resetTime;

        // Internal components
        private MeshRenderer _meshRenderer;
        private Collider _collider;

        // Internals
        private Vector3 _initialPosition;
        private bool _fallLock;
        private bool _resetLock;

        private readonly List<Coroutine> _coroutines = new();

        // Start is called before the first frame update
        void Start()
        {
            _initialPosition = gameObject.transform.position;
            _meshRenderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<Collider>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_fallLock && other.GetComponent<TriggerCollider>())
            {
                TriggerTrap();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_resetLock && other.GetComponent<TriggerCollider>())
            {
                ResetTrap();
            }
        }

        public void TriggerTrap()
        {
            _fallLock = true;
            StopActiveCoroutines();

            // Initial "instant" fall
            var targetPos = gameObject.transform.position + Vector3.down * onStepFallDistance;
            audioSource.PlayOneShot(Helper.GETRandomFromList(warnClips));
            _coroutines.Add(StartCoroutine(Helper.LerpPosition(gameObject.transform, targetPos, onStepFallTime)));

            // Actual fall
            _coroutines.Add(StartCoroutine(Helper.Wait(gracePeriodUntilFall, () =>
            {
                _resetLock = true;
                var deathTargetPos = gameObject.transform.position + Vector3.down * deathFallDistance;
                audioSource.PlayOneShot(Helper.GETRandomFromList(fallClips));
                _coroutines.Add(
                    StartCoroutine(Helper.LerpPosition(gameObject.transform, deathTargetPos, deathFallTime, () =>
                    {
                        // Hide "stone" -> kills player or npc on stone via fall
                        _meshRenderer.enabled = false;
                        _collider.enabled = false;

                        // Reset everything back to normal
                        _coroutines.Add(StartCoroutine(Helper.Wait(timeBeforeReset, ResetTrap)));
                    })));
            })));
        }

        public void ResetTrap()
        {
            StopActiveCoroutines();
            _meshRenderer.enabled = true;
            _collider.enabled = true;
            _coroutines.Add(StartCoroutine(Helper.LerpPosition(gameObject.transform, _initialPosition, resetTime)));
            _fallLock = false;
            _resetLock = false;
        }

        private void StopActiveCoroutines()
        {
            _coroutines.ForEach(StopCoroutine);
            _coroutines.Clear();
        }
    }
}