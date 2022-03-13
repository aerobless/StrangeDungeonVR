using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class DamageObject : MonoBehaviour
    {
        public float damagePerHit;
        public bool playerWeapon = false;

        private GameManager _gameManager;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public float GetDamagePoints()
        {
            //TODO: replace damageObject with interface and specific implementations
            if (playerWeapon)
            {
                return damagePerHit + _gameManager.variabilityManager.player.swordBaseDamage;
            }

            return damagePerHit;
        }

        public void RemoveDamageObject()
        {
            Destroy(this);
        }
    }
}