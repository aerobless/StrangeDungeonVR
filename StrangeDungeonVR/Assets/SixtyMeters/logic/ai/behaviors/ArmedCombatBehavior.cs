using System;
using System.Collections.Generic;
using RootMotion.Dynamics;
using SixtyMeters.logic.ai.appearance;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.player;
using SixtyMeters.logic.utilities;
using UnityEngine;
using AnimationInfo = SixtyMeters.logic.ai.appearance.AnimationInfo;

namespace SixtyMeters.logic.ai.behaviors
{
    public class ArmedCombatBehavior : UniversalAgentBehavior
    {
        // Settings
        private const float AttackRange = 1.5f;
        private const float AggressionRange = 10; //TODO: read from variabilityManager

        // Internals
        private PlayerActor _player;
        private bool _freshEngagement = true;
        private List<AnimationInfo> _attackAnimations = new();

        public ArmedCombatBehavior(UniversalAgent.BehaviorConfiguration configuration, UniversalAgent agent) : base(
            configuration, agent)
        {
        }

        public override bool CanBeExecuted()
        {
            LateSetup();
            var isPlayerInRange = IsPlayerInRange(AggressionRange);
            MarkFreshEngagement(isPlayerInRange);
            return isPlayerInRange;
        }

        private void MarkFreshEngagement(bool isPlayerInRange)
        {
            if (!isPlayerInRange)
            {
                _freshEngagement = true;
            }
        }

        private void LateSetup()
        {
            if (!_player)
            {
                _player = agent.gameManager.player;
                _attackAnimations.Add(AnimationIndex.SwordAttackOneHandedHorizontal);
                _attackAnimations.Add(AnimationIndex.SwordAttackCombo);
            }
        }

        public override void ExecuteUpdate()
        {
            agent.navMeshAgent.speed = agent.template.agentAttackMaxMovementSpeed;
            agent.nameTag.gameObject.SetActive(true);

            if (_freshEngagement)
            {
                _freshEngagement = false;
                PlayAnimationAndLock(AnimationIndex.BattleCryTaunt);
            }

            //agent.puppetMaster.mode = PuppetMaster.Mode.Active;
            //TODO: set aimIK for unarmed combat?
            agent.aimIK.solver.target = _player.head.transform;

            agent.navMeshAgent.SetDestination(_player.gameObject.transform.position);
            if (IsPlayerInRange(AttackRange))
            {
                PlayAnimationAndLock(Helper.GETRandomFromList(_attackAnimations));
            }
            else
            {
                agent.navMeshAgent.SetDestination(_player.gameObject.transform.position);
            }
        }

        private bool IsPlayerInRange(float range)
        {
            return Vector3.Distance(agent.transform.position, _player.gameObject.transform.position) <= range;
        }
    }
}