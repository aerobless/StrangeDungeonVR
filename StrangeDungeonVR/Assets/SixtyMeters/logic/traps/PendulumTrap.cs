using System.Collections;
using UnityEngine;

namespace SixtyMeters.logic.traps
{
    public class PendulumTrap : MonoBehaviour
    {
        // Setup
        public AudioSource audioSource;
        public AudioClip swingSound;

        // Settings
        public Quaternion startRotation;
        public Quaternion endRotation;

        public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        public float swingTime;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LerpRotation(transform, startRotation, endRotation, swingTime, curve,
                audioSource, swingSound));
        }

        // Update is called once per frame
        void Update()
        {
        }


        private static IEnumerator LerpRotation(Transform objectToMove, Quaternion startRotation,
            Quaternion endRotation, float duration, AnimationCurve curve, AudioSource audioSource, AudioClip swingSound)
        {
            float time = 0;
            objectToMove.localRotation = startRotation;
            while (true)
            {
                if (time is > 0.5f and < 0.7f && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(swingSound);
                }

                var linearStep = time / duration;
                var smooth = Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, curve.Evaluate(linearStep)));

                objectToMove.localRotation = Quaternion.Slerp(startRotation, endRotation, smooth);
                time += Time.deltaTime;

                if (objectToMove.localRotation == endRotation)
                {
                    var tempStartRotation = startRotation;
                    startRotation = endRotation;
                    endRotation = tempStartRotation;
                    time = 0;
                }

                yield return null;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}