using System.Collections.Generic;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
using SixtyMeters.logic.analytics;
using SixtyMeters.logic.generator;
using SixtyMeters.logic.interfaces.lifecycle;
using SixtyMeters.logic.player;
using SixtyMeters.logic.utilities;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.logic.ai
{
    public class HumanoidAgent : MonoBehaviour, ITrackedLifecycle
    {
        // Public
        public BehaviourPuppet puppet;
        public PuppetMaster puppetMaster;
        public GameObject rootObject; //Needed for destruction
        public AudioSource audioSource;
        public InteractionObject starterWeapon;

        // Internals set up at start
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private PlayerActor _player;
        private InteractionSystem _interactionSystem;
        private AimIK _aimIK;
        private StatisticsManager _statistics;

        // Internals dynamic
        private WayPoint _currentWaypoint;
        private float _nextMovementCheck;
        private readonly List<IDestructionListener> _destructionListener = new();
        private Vector3 _moveToLocation;

        // Constants
        private const float RateLimit = 1;
        private const float AttackRange = 2;
        private readonly Vector3 _animatedAimDirection = Vector3.forward;
        private const float DestinationReachedDistance = 2f;

        // Settings
        public float playerAggressionRange = 10;
        public float healthPoints = 100;
        public float despawnTimeAfterDeath = 5f;
        public float maxRandomMovementDistance = 15;

        // Indexed animator access
        private static readonly int Forward = Animator.StringToHash("Forward");
        private static readonly int SwingAttacks = Animator.StringToHash("SwingAttacks");

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _interactionSystem = GetComponent<InteractionSystem>();
            _statistics = GameManager.Instance.statisticsManager;
            _aimIK = GetComponent<AimIK>();
            _player = GameManager.Instance.player;
            _moveToLocation = transform.position;

            // Pickup weapon and enable puppet mode once setup is finished
            Invoke(nameof(PickupWeapon), 2f);
            Invoke(nameof(EnablePuppetMode), 5f);
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
                        _animator.SetBool(SwingAttacks, false);
                        _navMeshAgent.SetDestination(_player.gameObject.transform.position);
                    }
                    else if (distanceToPlayer <= AttackRange)
                    {
                        _aimIK.solver.target = _player.head.transform;
                        _animator.SetBool(SwingAttacks, true);
                    }
                    else
                    {
                        _animator.SetBool(SwingAttacks, false);
                        RandomMovement();
                    }

                    _nextMovementCheck = Time.time + RateLimit;
                }

                _animator.SetFloat(Forward, _navMeshAgent.velocity.magnitude * 0.25f);
            }
        }

        private void RandomMovement()
        {
            if (HasReachedDestination(DestinationReachedDistance, _moveToLocation))
            {
                // Select new wander location
                var randomDirection = Random.insideUnitSphere * maxRandomMovementDistance;
                randomDirection += transform.position;

                // So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
                // -1 means all areas: see https://docs.unity3d.com/Manual/nav-AreasAndCosts.html
                NavMesh.SamplePosition(randomDirection, out var hit, maxRandomMovementDistance, -1);

                // Check if we can reach the destination
                NavMeshPath calculatedPath = new NavMeshPath();
                _navMeshAgent.CalculatePath(hit.position, calculatedPath);
                if (calculatedPath.status == NavMeshPathStatus.PathComplete)
                {
                    _moveToLocation = hit.position;
                }
            }

            // Move to wander destination
            _navMeshAgent.SetDestination(_moveToLocation);
        }

        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.gameObject.transform.position);
        }


        private bool HasReachedDestination(float reqDistance, Vector3 destination)
        {
            return Vector3.Distance(transform.position, destination) <= reqDistance;
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
            _destructionListener.ForEach(listener => listener.ObjectDestroyed(gameObject));
            ++_statistics.enemiesKilled;
            Destroy(rootObject, despawnTimeAfterDeath);
        }

        private void PickupWeapon()
        {
            if (starterWeapon)
            {
                _interactionSystem.StartInteraction(FullBodyBipedEffector.RightHand, starterWeapon, true);
            }
        }

        private void EnablePuppetMode()
        {
            puppetMaster.mode = PuppetMaster.Mode.Active;
            Debug.Log("Setup finished! Puppet for " + name + " is now active!");
        }

        /**
         * Used to make sure the head & hand aim are directed toward the target
         */
        void LateUpdate()
        {
            _aimIK.solver.axis =
                _aimIK.solver.transform.InverseTransformVector(_aimIK.transform.rotation * _animatedAimDirection);
        }

        public void RegisterDestructionListener(IDestructionListener destructionListener)
        {
            _destructionListener.Add(destructionListener);
        }
    }
}