using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class CombatMarker : MonoBehaviour
    {
        // Components
        public Material ghostMaterial;
        public Material successMaterial;
        public Material failureMaterial;

        public AudioSource audioSource;
        public List<AudioClip> successSounds;
        public List<AudioClip> failureSounds;

        // Internal components
        private GameManager _gameManager;
        private BoxCollider _collider;
        private MeshRenderer _renderer;

        // Internals
        private PlayerWeapon _respondingPlayerWeapon;
        private bool _isCompliant;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _collider = GetComponent<BoxCollider>();
            _renderer = GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isCompliant && WeaponIsInComplyingPosition(_respondingPlayerWeapon))
            {
                // fireworks because compliance was reached
                _isCompliant = true;
                var grabbable = _respondingPlayerWeapon.GetComponent<HVRGrabbable>();
                _gameManager.controllerFeedback.VibrateHand(grabbable, ControllerFeedbackHelper.CombatMarkerSuccess);
                _renderer.material = successMaterial;
                audioSource.PlayOneShot(Helper.GETRandomFromList(successSounds));
                StartCoroutine(Helper.Wait(0.2f, Reset));
            }

            //TODO: Make player hold position instead of just complying once
        }
        
        //TODO: show timer to player
        //TODO: show effect on success
        //TODO: sound effect on fit/success etc.

        public void Activate(float timeToRespond, float damageOnFailure)
        {
            gameObject.SetActive(true);
            StartCoroutine(Helper.Wait(timeToRespond, () =>
            {
                if (!_isCompliant)
                {
                    _renderer.material = failureMaterial;
                    audioSource.PlayOneShot(Helper.GETRandomFromList(failureSounds));
                    _gameManager.player.ApplyDirectDamage(damageOnFailure);
                }

                StartCoroutine(Helper.Wait(0.2f, Reset));
            }));
        }

        private bool WeaponIsInComplyingPosition(PlayerWeapon weapon)
        {
            // Check that weapon actually exists
            if (!weapon)
            {
                return false;
            }

            // Validate that all validation points are within bounds
            var isValid = true;
            weapon.validationPoints.ForEach(point =>
            {
                if (!_collider.bounds.Contains(point.transform.position))
                {
                    isValid = false;
                }
            });
            return isValid;
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            _respondingPlayerWeapon = null;
            _isCompliant = false;
            _renderer.material = ghostMaterial;
        }

        private void OnTriggerEnter(Collider other)
        {
            var complyingPlayerWeapon = other.GetComponent<PlayerWeapon>();
            if (complyingPlayerWeapon)
            {
                _respondingPlayerWeapon = complyingPlayerWeapon;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerWeapon>())
            {
                _respondingPlayerWeapon = null;
            }
        }
    }
}