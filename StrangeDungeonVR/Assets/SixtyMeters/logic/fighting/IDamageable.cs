using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public interface IDamageable
    {
        public void ApplyDirectDamage(float incomingDmg);

        public void ApplyDamage(DamageObject damageObject, float relativeVelocityMagnitude, Vector3 pointOfImpact);
    }
}