using SixtyMeters.logic.fighting;
using UnityEngine;

namespace SixtyMeters.logic.ai
{
    public class DamageRelay : MonoBehaviour, IDamageable
    {
        private IDamageable _damageReceiver;

        public void Setup(IDamageable damageable)
        {
            _damageReceiver = damageable;
        }

        public void ApplyDirectDamage(float incomingDmg)
        {
            _damageReceiver.ApplyDirectDamage(incomingDmg);
        }

        public void ApplyDamage(DamageObject damageObject, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            _damageReceiver.ApplyDamage(damageObject, relativeVelocityMagnitude, pointOfImpact);
        }
    }
}