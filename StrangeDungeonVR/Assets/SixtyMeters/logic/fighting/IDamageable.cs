using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public interface IDamageable
    {
        public void ApplyDirectDamage(float incomingDmg);

        public void ApplyDamage(float incomingDmg, float relativeVelocityMagnitude, Vector3 pointOfImpact);
    }
}