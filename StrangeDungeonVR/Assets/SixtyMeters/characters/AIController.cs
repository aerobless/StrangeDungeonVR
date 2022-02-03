using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.characters
{
    public class AIController : MonoBehaviour
    {
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;

        public BehaviourPuppet puppet;

        public WayPoint testWaypoint1;
        public WayPoint testWaypoint2;

        private WayPoint _currentWaypoint;

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _currentWaypoint = testWaypoint1;
        }

        // Update is called once per frame
        void Update()
        {
            // Keep the agent disabled while the puppet is unbalanced.
            _navMeshAgent.enabled = puppet.state == BehaviourPuppet.State.Puppet;

            // Update agent destination and Animator
            if (_navMeshAgent.enabled)
            {
                UpdateDestination();
                _animator.SetFloat("Forward", _navMeshAgent.velocity.magnitude * 0.25f);
            }
        }

        private void UpdateDestination()
        {
            //Logic for testing only
            _navMeshAgent.SetDestination(_currentWaypoint.transform.position);
            if (HasReachedCurrentWayPoint(2f) && _currentWaypoint == testWaypoint1)
            {
                _currentWaypoint = testWaypoint2;
            }

            if (HasReachedCurrentWayPoint(2f) && _currentWaypoint == testWaypoint2)
            {
                _currentWaypoint = testWaypoint1;
            }
        }


        private bool HasReachedCurrentWayPoint(float distance)
        {
            if (_currentWaypoint != null)
            {
                return Vector3.Distance(transform.position, _currentWaypoint.transform.position) <= distance;
            }

            return false;
        }
    }
}