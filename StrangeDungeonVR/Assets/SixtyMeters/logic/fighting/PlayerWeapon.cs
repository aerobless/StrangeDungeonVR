using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class PlayerWeapon : MonoBehaviour
    {
        // Settings
        public float baseDamage;
        public GameObject dmgTextImpactPoint;
        public List<AudioClip> slashImpactSounds;
        public AudioSource audioSource;

        // Components
        public List<CombatMarkerValidationPoint> validationPoints;

        // Internals
        private Rigidbody _rigidbody;
        private GameManager _gameManager;
        private HVRGrabbable _grabbable;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
            _rigidbody = GetComponent<Rigidbody>();
            _grabbable = GetComponent<HVRGrabbable>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SwordImpactEnter(Collider coll)
        {
            var hitbox = coll.GetComponent<VirtualAgentHitbox>();
            if (hitbox)
            {
                hitbox.ApplyDamage(baseDamage, _rigidbody.velocity.magnitude, dmgTextImpactPoint.transform.position);
                _gameManager.controllerFeedback.VibrateHand(_grabbable, ControllerFeedbackHelper.ImpactVibration);
                audioSource.PlayOneShot(Helper.GETRandomFromList(slashImpactSounds));
                //TODO: sound effect
            }
        }
    }
}