using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class CombatMarker : MonoBehaviour
    {
        // Internal components
        private GameManager _gameManager;
        private BoxCollider _collider;

        // Internals
        private PlayerWeapon _complyingPlayerWeapon;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _collider = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        //TODO: change green colors when valid
        //TODO: show timer to player
        //TODO: show effect on success
        //TODO: sound effect on fit/success etc.

        public void Activate(float timeToRespond, float damageOnFailure)
        {
            gameObject.SetActive(true);
            StartCoroutine(Helper.Wait(timeToRespond, () =>
            {
                if (!WeaponIsInComplyingPosition(_complyingPlayerWeapon))
                {
                    _gameManager.player.ApplyDirectDamage(damageOnFailure);
                }

                Reset();
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
            _complyingPlayerWeapon = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            var complyingPlayerWeapon = other.GetComponent<PlayerWeapon>();
            if (complyingPlayerWeapon)
            {
                _complyingPlayerWeapon = complyingPlayerWeapon;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerWeapon>())
            {
                _complyingPlayerWeapon = null;
            }
        }
    }
}