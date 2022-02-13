using SixtyMeters.logic.fighting;
using UnityEngine;

namespace SixtyMeters.logic.ai
{
    public class DamageRelay : MonoBehaviour, IDamageable
    {
        public HumanoidAgentDamageable damageReceiver;

        public void ApplyDamage(float incomingDmg)
        {
            damageReceiver.ApplyDamage(incomingDmg);
        }
    }
}