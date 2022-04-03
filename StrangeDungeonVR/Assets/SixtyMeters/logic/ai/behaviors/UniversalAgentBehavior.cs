using System;
using System.Collections.Generic;
using UnityEngine;
using AnimationInfo = SixtyMeters.logic.ai.appearance.AnimationInfo;

namespace SixtyMeters.logic.ai.behaviors
{
    public abstract class UniversalAgentBehavior
    {
        private readonly UniversalAgent.BehaviorConfiguration _configuration;
        protected UniversalAgent agent;

        private float _timer = 0f;
        private readonly List<int> _locks = new();

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

        private bool TimerHasReachedSeconds(float waitTimeInSeconds)
        {
            _timer += Time.deltaTime;

            var timerHasReachedSeconds = _timer > waitTimeInSeconds;
            if (timerHasReachedSeconds)
            {
                // Reset timer after use
                _timer -= waitTimeInSeconds;
            }

            return timerHasReachedSeconds;
        }

        /// <summary>
        /// Starts the desired actions and waits for x seconds before trying again.
        /// </summary>
        /// <param name="action">the action to be invoked</param>
        /// <param name="seconds">the time to wait before calling it again</param>
        protected void InvokeActionAndLockForSeconds(Action action, float seconds)
        {
            if (!_locks.Contains(action.GetHashCode()))
            {
                _locks.Add(action.GetHashCode());
                action.Invoke();
            }
            else
            {
                if (TimerHasReachedSeconds(seconds))
                {
                    _locks.Remove(action.GetHashCode());
                }
            }
        }

        protected void PlayAnimationAndLock(AnimationInfo animationInfo)
        {
            agent.animator.SetTrigger(animationInfo.Id);
            agent.LockBehaviorExecution(animationInfo.Length);
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