using RootMotion.Dynamics;
using SixtyMeters.logic.player;
using UnityEngine;

namespace SixtyMeters.logic.ai.behaviors
{
    public class AttackPlayerBehavior : UniversalAgentBehavior
    {
        // Settings
        private const float AttackRange = 2;
        private const float AggressionRange = 10; //TODO: read from variabilityManager

        // Internals
        private PlayerActor _player;

        // Animations
        private static readonly int SwingAttacks = Animator.StringToHash("SwingAttacks");

        public AttackPlayerBehavior(UniversalAgent.BehaviorConfiguration configuration, UniversalAgent agent) : base(
            configuration, agent)
        {
        }

        public override bool CanBeExecuted()
        {
            LateSetup();
            return IsPlayerInRange(AggressionRange);
        }

        private void LateSetup()
        {
            if (!_player)
            {
                _player = agent.gameManager.player;
            }
        }
        
        public override void ExecuteUpdate()
        {
            agent.puppetMaster.mode = PuppetMaster.Mode.Active;
            
            if (IsPlayerInRange(AttackRange))
            {
                agent.aimIK.solver.target = _player.head.transform;
                agent.animator.SetBool(SwingAttacks, true);
            }
            else
            {
                agent.animator.SetBool(SwingAttacks, false);
                agent.navMeshAgent.SetDestination(_player.gameObject.transform.position);
            }
        }

        private bool IsPlayerInRange(float range)
        {
            return Vector3.Distance(agent.transform.position, _player.gameObject.transform.position) <= range;
        }
    }
}