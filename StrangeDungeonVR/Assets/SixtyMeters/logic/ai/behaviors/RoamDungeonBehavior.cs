using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.logic.ai.behaviors
{
    public class RoamDungeonBehavior : UniversalAgentBehavior
    {
        // Settings
        private const float DestinationReachedDistance = 2f;
        private float maxRandomMovementDistance = 15;

        // Internals
        private Vector3 _moveToLocation;

        public RoamDungeonBehavior(UniversalAgent.BehaviorConfiguration configuration, UniversalAgent agent) : base(
            configuration, agent)
        {
            _moveToLocation = agent.transform.position;
        }

        public override bool CanBeExecuted()
        {
            return true;
        }

        public override void ExecuteUpdate()
        {
            agent.nameTag.gameObject.SetActive(false);
            RandomMovement();
        }

        private void RandomMovement()
        {
            agent.navMeshAgent.speed = agent.template.agentRoamMaxMovementSpeed;
            agent.puppetMaster.mode = PuppetMaster.Mode.Kinematic;
            
            if (HasReachedDestination(DestinationReachedDistance, _moveToLocation))
            {
                // Select new wander location
                var randomDirection = Random.insideUnitSphere * maxRandomMovementDistance;
                randomDirection += agent.transform.position;

                // So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
                // -1 means all areas: see https://docs.unity3d.com/Manual/nav-AreasAndCosts.html
                NavMesh.SamplePosition(randomDirection, out var hit, maxRandomMovementDistance, -1);

                // Check if we can reach the destination
                NavMeshPath calculatedPath = new NavMeshPath();
                agent.navMeshAgent.CalculatePath(hit.position, calculatedPath);
                if (calculatedPath.status == NavMeshPathStatus.PathComplete)
                {
                    _moveToLocation = hit.position;
                }
            }

            // Move to wander destination
            agent.navMeshAgent.SetDestination(_moveToLocation);
        }

        private bool HasReachedDestination(float reqDistance, Vector3 destination)
        {
            return Vector3.Distance(agent.transform.position, destination) <= reqDistance;
        }
    }
}