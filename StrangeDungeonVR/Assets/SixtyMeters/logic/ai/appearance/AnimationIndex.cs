using UnityEngine;

namespace SixtyMeters.logic.ai.appearance
{
    static class AnimationIndex
    {
        // General
        public static readonly int TriggerType = Animator.StringToHash("TriggerType");
        public static readonly int FireTrigger = Animator.StringToHash("Trigger");

        // Triggers
        public static readonly AnimationInfo ZombieTwoHandedTopDownAttack = new(1, 4.5f);
        public static readonly AnimationInfo ZombieOneHandedSideAttack = new(2, 2.6f);

        public static readonly AnimationInfo BattleCryTaunt = new(3, 2.85f);

        public static readonly AnimationInfo ImpactHeadHit = new(4, 1.54f);
        public static readonly AnimationInfo ImpactCenterSwordHit = new(5, 0.7f);
        
        public static readonly AnimationInfo SwordAttackOneHandedHorizontal = new(6, 2.4f);
        public static readonly AnimationInfo SwordAttackCombo = new(7, 4.2f);


        // Directional states
        public static readonly int GroundedDirectionalZombie = Animator.StringToHash("Grounded Directional Zombie");
    }

    public class AnimationInfo
    {
        public readonly int Id;
        public readonly float Length;

        public AnimationInfo(int animationId, float length)
        {
            Id = animationId;
            Length = length;
        }
    }
}