using System;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class Hitbox : MonoBehaviour
    {
        // Internal Settings
        private readonly float _minVelocityForDamage = 1;

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
        }
    }
}