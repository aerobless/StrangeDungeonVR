using UnityEngine;

namespace SixtyMeters.logic.ai.behaviors
{
    public abstract class UniversalAgentBehavior
    {
        private readonly UniversalAgent.BehaviorConfiguration _configuration;
        protected UniversalAgent agent;

        protected UniversalAgentBehavior(UniversalAgent.BehaviorConfiguration configuration, UniversalAgent agent)
        {
            _configuration = configuration;
            this.agent = agent;
        }

        /// <summary>
        /// The priority of the behavior as configured. The higher the priority the more likely it is that the behavior
        /// will be triggered.
        /// </summary>
        /// <returns>the priority, higher = more important</returns>
        public int GetPriority()
        {
            return _configuration.priority;
        }

        /// <summary>
        /// Whether the behavior can be executed at this time, e.g. a weapon can only be picked up if it's not
        /// already held.
        /// </summary>
        /// <returns>true if the behavior makes sense to execute</returns>
        public abstract bool CanBeExecuted();

        /// <summary>
        /// The logic of the behavior that is executed while a behavior has priority.
        /// </summary>
        public abstract void ExecuteUpdate();
        
        
        
    }
}