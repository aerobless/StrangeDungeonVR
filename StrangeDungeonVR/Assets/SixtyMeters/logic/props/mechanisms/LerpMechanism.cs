using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.props.mechanisms
{
    public class LerpMechanism : MonoBehaviour
    {
        // Settings
        public GameObject objectToMove;
        public Vector3 endPosition;
        public float triggerDuration;
        public float resetDuration;

        public List<AudioClip> startSound;
        public List<AudioClip> resetSound;

        // Components
        public AudioSource audioSource;

        // Internals
        private Vector3 _startPosition;

        // Start is called before the first frame update
        void Start()
        {
            _startPosition = objectToMove.transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Trigger()
        {
            Helper.PlayRandomIfExists(audioSource, startSound);
            StartCoroutine(Helper.LerpPosition(objectToMove.transform, endPosition, triggerDuration));
        }

        public void Reset()
        {
            Helper.PlayRandomIfExists(audioSource, resetSound);
            StartCoroutine(Helper.LerpPosition(objectToMove.transform, _startPosition, resetDuration));
        }
    }
}