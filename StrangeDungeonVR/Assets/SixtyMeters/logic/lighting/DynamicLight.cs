using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.logic.lighting
{
    public class DynamicLight : MonoBehaviour
    {
        public Light lightSource;

        [Tooltip("Minimum random light intensity")]
        public float minIntensity = 0f;

        [Tooltip("Maximum random light intensity")]
        public float maxIntensity = 1f;

        [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")] [Range(1, 50)]
        public int smoothing = 5;

        private Queue<float> _smoothQueue;
        private float _lastSum = 0;

        // Start is called before the first frame update
        void Start()
        {
            _smoothQueue = new Queue<float>(smoothing);
            InvokeRepeating(nameof(Flicker), 0, 0.1f);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void Flicker()
        {
            // pop off an item if too big
            while (_smoothQueue.Count >= smoothing)
            {
                _lastSum -= _smoothQueue.Dequeue();
            }

            // Generate random new item, calculate new average
            float newVal = Random.Range(minIntensity, maxIntensity);
            _smoothQueue.Enqueue(newVal);
            _lastSum += newVal;

            // Calculate new smoothed average
            lightSource.intensity = _lastSum / _smoothQueue.Count;
        }
    }
}