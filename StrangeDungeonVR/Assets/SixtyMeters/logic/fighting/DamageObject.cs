using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class DamageObject : MonoBehaviour
    {
        public float damagePerHit;
        public bool playerWeapon = false;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public float GetDamagePoints()
        {
            return damagePerHit;
        }

        public void RemoveDamageObject()
        {
            Destroy(this);
        }
    }
}