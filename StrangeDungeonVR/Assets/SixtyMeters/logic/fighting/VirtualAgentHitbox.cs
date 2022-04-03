using SixtyMeters.logic.ai;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class VirtualAgentHitbox : MonoBehaviour
    {
        public UniversalAgent damageable;

        public void ApplyDamage(float incomingDmg, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            damageable.ApplyDamage(incomingDmg, relativeVelocityMagnitude, pointOfImpact);
        }
    }
}