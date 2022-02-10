using RootMotion.Dynamics;
using SixtyMeters.logic.fighting;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.logic.ai
{
    public class HumanoidAgent : MonoBehaviour
    {
        // Public
        public BehaviourPuppet puppet;
        public PuppetMaster puppetMaster;
        public AudioSource audioSource;

        // Internals set up at start
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private GameObject _player;

        // Internals dynamic
        private WayPoint _currentWaypoint;
        private float _nextMovementCheck;

        // Constants
        private const float RateLimit = 1;
        private const float AttackRange = 2;

        // Settings
        public float playerAggressionRange = 10;
        public float healthPoints = 100;
        public float despawnTimeAfterDeath = 5f;

        // Indexed animator access
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int SwordAttack = Animator.StringToHash("SwordAttack");

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _player = GameObject.Find("PlayerController");
        }

        // Update is called once per frame
        void Update()
        {
            // Keep the agent disabled while the puppet is unbalanced.
            _navMeshAgent.enabled = puppet.state == BehaviourPuppet.State.Puppet;

            // Update agent destination and Animator
            if (_navMeshAgent.enabled)
            {
                if (Time.time > _nextMovementCheck)
                {
                    //TODO: more realistic player detection system. e.g. raycast
                    var distanceToPlayer = GetDistanceToPlayer();
                    if (distanceToPlayer < playerAggressionRange && distanceToPlayer > AttackRange)
                    {
                        _navMeshAgent.SetDestination(_player.transform.position);
                    }
                    else if (distanceToPlayer <= AttackRange)
                    {
                        //_animator.SetTrigger(SwordAttack);
                    }
                    else
                    {
                        //TODO: wander
                    }

                    _nextMovementCheck = Time.time + RateLimit;
                }

                _animator.SetFloat(Forward, _navMeshAgent.velocity.magnitude * 0.25f);
            }
        }

        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }

        private bool HasReachedCurrentWayPoint(float distance)
        {
            if (_currentWaypoint != null)
            {
                return Vector3.Distance(transform.position, _currentWaypoint.transform.position) <= distance;
            }

            return false;
        }

        public void Die()
        {
            puppetMaster.Kill();
            Destroy(gameObject, despawnTimeAfterDeath);
        }
    }
}