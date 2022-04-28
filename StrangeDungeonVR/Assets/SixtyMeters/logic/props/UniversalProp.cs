using System.Collections.Generic;
using HurricaneVR.Framework.Components;
using HurricaneVR.Framework.Core;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.props
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class UniversalProp : MonoBehaviour
    {
        [System.Serializable]
        public class Appearance
        {
            public GameObject originalRoot;
            public Collider originalCollider;

            public GameObject destroyedRoot;
        }

        // Internal components
        private AudioSource _audioSource;
        private Appearance _selectedAppearance;

        // Settings
        public List<AudioClip> destroyedSound;
        public List<Appearance> appearances;

        public float forceThreshold = 3;
        public float explosionPower = 1;
        public bool removeDebris = true;
        public float removeDebrisTimerUpper = 10f;
        public float removeDebrisTimerLower = 5f;
        public bool ignorePlayerCollision = true;

        // Start is called before the first frame update
        void Start()
        {
            if (appearances.Count > 1)
            {
                appearances.ForEach(ap => ap.originalRoot.SetActive(false));
                _selectedAppearance = Helper.GETRandomFromList(appearances);
                _selectedAppearance.originalRoot.SetActive(true);
            }
            else
            {
                _selectedAppearance = appearances[0];
            }

            _audioSource = GetComponent<AudioSource>();
            _audioSource.spatialBlend = 1;
            var collisionObject = _selectedAppearance.originalCollider.gameObject.AddComponent<CollisionDelegation>();
            collisionObject.onCollisionEnter.AddListener(OnDelegatedCollision);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnDelegatedCollision(Collision collision)
        {
            var lastImpulse = collision.impulse.magnitude;
            var forceMet = lastImpulse > forceThreshold;
            
            if (forceMet)
            {
                Destroy();
            }
        }

        public void Destroy()
        {
            _audioSource.PlayOneShot(Helper.GETRandomFromList(destroyedSound));
            _selectedAppearance.originalRoot.SetActive(false);
            if (_selectedAppearance.destroyedRoot)
            {
                var destroyed = Instantiate(_selectedAppearance.destroyedRoot,
                    _selectedAppearance.originalRoot.transform.position,
                    _selectedAppearance.originalRoot.transform.rotation);
                destroyed.SetActive(true);

                foreach (var rigidBody in destroyed.GetComponentsInChildren<Rigidbody>())
                {
                    var v = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    rigidBody.AddForce(v * explosionPower, ForceMode.VelocityChange);

                    if (removeDebris)
                    {
                        var delay = Random.Range(removeDebrisTimerLower, removeDebrisTimerUpper);
                        if (delay < .1f)
                        {
                            delay = 3f;
                        }

                        var timer = rigidBody.gameObject.AddComponent<HVRDestroyTimer>();
                        timer.StartTimer(delay);
                    }

                    if (ignorePlayerCollision)
                    {
                        var colliders = rigidBody.gameObject.GetComponentsInChildren<Collider>();
                        HVRManager.Instance?.IgnorePlayerCollision(colliders);
                    }

                    rigidBody.transform.parent = null;
                }

                if (removeDebris)
                {
                    var timer = destroyed.gameObject.AddComponent<HVRDestroyTimer>();
                    var delay = removeDebrisTimerUpper;
                    if (delay <= .1f)
                        delay = 3f;
                    timer.StartTimer(delay);
                }
            }

            Destroy(gameObject, 2);
        }
    }
}