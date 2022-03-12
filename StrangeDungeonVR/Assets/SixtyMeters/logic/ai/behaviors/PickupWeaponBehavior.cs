using RootMotion.FinalIK;

namespace SixtyMeters.logic.ai.behaviors
{
    public class PickupWeaponBehavior : UniversalAgentBehavior
    {
        private bool _canBePickedUpTemp = true; //TODO: handle state, so that weapon can be dropped

        public override bool CanBeExecuted()
        {
            return agent.weapon && _canBePickedUpTemp;
        }

        public override void ExecuteUpdate()
        {
            //TODO: add logic to walk to weapon
            agent.interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, agent.weapon, true);
            _canBePickedUpTemp = false;
        }

        public PickupWeaponBehavior(UniversalAgent.BehaviorConfiguration configuration, UniversalAgent agent) : base(
            configuration, agent)
        {
        }
    }
}