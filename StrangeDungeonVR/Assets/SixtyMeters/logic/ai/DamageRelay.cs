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

        public void ApplyDamage(float incomingDmg, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            _damageReceiver.ApplyDamage(incomingDmg, relativeVelocityMagnitude, pointOfImpact);
        }
    }
}