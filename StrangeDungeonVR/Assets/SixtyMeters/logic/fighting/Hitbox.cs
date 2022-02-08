using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class Hitbox : MonoBehaviour
    {
        private Damageable _dmgListener;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void SetDmgListener(Damageable listener)
        {
            _dmgListener = listener;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.GetComponent<DamageObject>())
            {
                _dmgListener.ApplyDamage(other.gameObject.GetComponent<DamageObject>().GetDamage());
            }
        }
    }
}