using RootMotion.Dynamics;
using RootMotion.FinalIK;
using SixtyMeters.logic.player;
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
        public InteractionObject starterWeapon;

        // Internals set up at start
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private PlayerActor _player;
        private InteractionSystem _interactionSystem;
        private AimIK _aimIK;

        // Internals dynamic
        private WayPoint _currentWaypoint;
        private float _nextMovementCheck;

        // Constants
        private const float RateLimit = 1;
        private const float AttackRange = 2;
        private readonly Vector3 _animatedAimDirection = Vector3.forward;

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
            _interactionSystem = GetComponent<InteractionSystem>();
            _aimIK = GetComponent<AimIK>();
            _player = GameObject.Find("PlayerController").GetComponent<PlayerActor>();

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
                        _navMeshAgent.SetDestination(_player.gameObject.transform.position);
                    }
                    else if (distanceToPlayer <= AttackRange)
                    {
                        _aimIK.solver.target = _player.head.transform;
                        _animator.SetTrigger(SwordAttack);
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
            return Vector3.Distance(transform.position, _player.gameObject.transform.position);
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
    }
}