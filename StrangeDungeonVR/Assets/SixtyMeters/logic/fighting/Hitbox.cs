using System;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class Hitbox : MonoBehaviour
    {
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
                _dmgListener.ApplyDamage(other.gameObject.GetComponent<DamageObject>().GetDamage());
                Debug.Log("Impact dmg object");
            }

            Debug.Log("Impact");
        }
    }
}