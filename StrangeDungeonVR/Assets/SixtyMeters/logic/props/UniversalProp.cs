using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.props
{
    [RequireComponent(typeof(AudioSource))]
    public class UniversalProp : MonoBehaviour
    {
        [System.Serializable]
        public class Appearance
        {
            [Tooltip("The prop in its original state")]
            public GameObject originalState;

            public GameObject destroyedState;
        }


        // Internal components
        private AudioSource _audioSource;
        private Appearance _selectedAppearance;

        // Settings
        public List<AudioClip> destroyedSound;
        public List<Appearance> appearances;

        // Start is called before the first frame update
        void Start()
        {
            if (appearances.Count > 1)
            {
                appearances.ForEach(ap => ap.originalState.SetActive(false));
                _selectedAppearance = Helper.GETRandomFromList(appearances);
                _selectedAppearance.originalState.SetActive(true);
            }
            else
            {
                _selectedAppearance = appearances[0];
            }

            _audioSource = GetComponent<AudioSource>();
            var collisionObject = _selectedAppearance.originalState.AddComponent<CollisionDelegation>();
            collisionObject.onCollisionEnter.AddListener(OnDelegatedCollision);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnDelegatedCollision(Collision collision)
        {
            //TODO: test good value
            if (collision.impulse.magnitude > 0.1)
            {
                _audioSource.PlayOneShot(Helper.GETRandomFromList(destroyedSound));
                _selectedAppearance.originalState.SetActive(false);
                _selectedAppearance.destroyedState.SetActive(true);   
            }
            //TODO: make destroyed object(s) slowly disappear
        }
    }
}