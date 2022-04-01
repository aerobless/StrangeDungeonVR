using RootMotion.Dynamics;

namespace SixtyMeters.logic.ai.behaviors
{
    /// <summary>
    /// A simple idle behavior where the agent does absolutely nothing but standing around.
    /// Mainly useful for testing.
    /// </summary>
    public class IdleBehavior : UniversalAgentBehavior
    {
        public IdleBehavior(UniversalAgent.BehaviorConfiguration configuration, UniversalAgent agent) : base(
            configuration, agent)
        {
        }

        public override bool CanBeExecuted()
        {
            return true;
        }

        public override void ExecuteUpdate()
        {
            agent.puppetMaster.mode = PuppetMaster.Mode.Active;
        }
    }
}