using System;
using RootMotion.Dynamics;
using RootMotion.FinalIK;

namespace SixtyMeters.logic.ai.behaviors
{
    public class PickupWeaponBehavior : UniversalAgentBehavior
    {
        public override bool CanBeExecuted()
        {
            return agent.weapon && agent.weapon.transform.parent != agent.rightHand.transform;
        }

        public override void ExecuteUpdate()
        {
            agent.puppetMaster.mode = PuppetMaster.Mode.Kinematic;
            InvokeActionAndLockForSeconds(AttemptToPickupWeapon(), 2);
        }

        private Action AttemptToPickupWeapon()
        {
            return () =>
            {
                agent.interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, agent.weapon, true);
            };
        }

        public PickupWeaponBehavior(UniversalAgent.BehaviorConfiguration configuration, UniversalAgent agent) : base(
            configuration, agent)
        {
        }
    }
}