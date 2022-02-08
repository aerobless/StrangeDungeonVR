using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class DamageObject : MonoBehaviour
    {
        public float damagePerHit;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public float GetDamage()
        {
            return damagePerHit;
        }
    }
}