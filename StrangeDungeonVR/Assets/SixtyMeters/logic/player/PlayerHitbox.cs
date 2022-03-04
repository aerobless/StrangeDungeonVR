using System;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.variability.effects;
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
            if (other.gameObject.GetComponent<DamageObject>())
            {
                var baseDmgPoints = other.gameObject.GetComponent<DamageObject>().GetDamagePoints();
                _dmgListener.ApplyDirectDamage(baseDmgPoints);
            }

            if (other.gameObject.GetComponent<VariabilityEffect>())
            {
                other.gameObject.GetComponent<VariabilityEffect>().ApplyEffect();
            }
        }
    }
}