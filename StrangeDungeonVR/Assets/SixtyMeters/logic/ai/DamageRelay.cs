using SixtyMeters.logic.fighting;
using UnityEngine;

namespace SixtyMeters.logic.ai
{
    public class DamageRelay : MonoBehaviour, IDamageable
    {
        public HumanoidAgentDamageable damageReceiver;

        public void ApplyDirectDamage(float incomingDmg)
        {
            damageReceiver.ApplyDirectDamage(incomingDmg);
        }

        public void ApplyDamage(DamageObject damageObject, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            damageReceiver.ApplyDamage(damageObject, relativeVelocityMagnitude, pointOfImpact);
        }
    }
}