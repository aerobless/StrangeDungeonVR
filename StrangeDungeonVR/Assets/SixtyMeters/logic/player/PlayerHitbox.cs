using SixtyMeters.logic.fighting;
using SixtyMeters.logic.interfaces;
using UnityEngine;

namespace SixtyMeters.logic.player
{
    public class PlayerHitbox : MonoBehaviour
    {
        // Internal Dynamics
        private IDamageable _dmgListener;

        // Start is called before the first frame update
        void Start()
        {
            _dmgListener = GetComponentInParent<IDamageable>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            var damageObject = other.gameObject.GetComponent<DamageObject>();
            if (damageObject && damageObject.enabled)
            {
                var baseDmgPoints = damageObject.GetDamagePoints();
                _dmgListener.ApplyDirectDamage(baseDmgPoints);
            }

            if (other.gameObject.GetComponent<IConsumable>() != null)
            {
                other.gameObject.GetComponent<IConsumable>().InRangeForConsumption(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<IConsumable>() != null)
            {
                other.gameObject.GetComponent<IConsumable>().InRangeForConsumption(false);
            }
        }
    }
}