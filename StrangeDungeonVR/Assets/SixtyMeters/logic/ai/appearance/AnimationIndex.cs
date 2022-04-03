using UnityEngine;

namespace SixtyMeters.logic.ai.appearance
{
    static class AnimationIndex
    {
        public static readonly int SwingAttacks = Animator.StringToHash("SwingAttacks");

        // Triggers
        public static readonly AnimationInfo ZombieTwoHandedTopDownAttack = new("ZombieTopDownAttack", 4.5f);
        public static readonly AnimationInfo ZombieOneHandedSideAttack = new("ZombieSideAttack", 2.6f);
        
        public static readonly AnimationInfo BattleCryTaunt = new("BattlecryTaunt", 2.85f);
        
        public static readonly AnimationInfo ImpactHeadHit = new("ImpactHeadHit", 1.54f);
        public static readonly AnimationInfo ImpactCenterSwordHit = new("ImpactSwordHit", 0.7f);


        // Directional states
        public static readonly int GroundedDirectionalZombie = Animator.StringToHash("Grounded Directional Zombie");
    }

    public class AnimationInfo
    {
        public readonly int Id;
        public readonly float Length;

        public AnimationInfo(string animationId, float length)
        {
            Id = Animator.StringToHash(animationId);
            Length = length;
        }
    }
}