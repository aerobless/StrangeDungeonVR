using System.Collections.Generic;
using System.Linq;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
using SixtyMeters.logic.ai.appearance;
using SixtyMeters.logic.ai.behaviors;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.interfaces.lifecycle;
using SixtyMeters.logic.utilities;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.logic.ai
{
    public class UniversalAgent : MonoBehaviour, ITrackedLifecycle, IDamageable
    {
        // Components
        public InteractionObject weapon;
        public GameObject rightHand;
        public GameObject leftHand;
        public BehaviourPuppet puppet;
        public PuppetMaster puppetMaster;
        public GameObject rootObject; //Needed for destruction
        public SkinnedMeshRenderer meshRenderer;
        public AudioSource audioSource;
        public AudioSource audioSourceFeet;
        public Material dmgMaterial;
        public List<AudioClip> dmgSounds;
        public List<AudioClip> footStepSounds;

        // Internal components
        [HideInInspector] public GameManager gameManager;

        [HideInInspector] public NavMeshAgent navMeshAgent;

        [HideInInspector] public Animator animator;

        [HideInInspector] public InteractionSystem interactionSystem;

        [HideInInspector] public AimIK aimIK;

        private GameObject _damageTextObject;
        private UniversalAgentAppearance _appearance;

        // Internals
        private readonly List<UniversalAgentBehavior> _behaviors = new();
        private readonly List<IDestructionListener> _destructionListener = new();
        private Material _originalMaterial;
        private float _healthPoints = 100; //TODO: read from variability manager
        private bool _hitLocked;
        private bool _isDead;
        private float _startingHeight; // Used to determine if agent is falling out of map

        // Settings
        public string agentTemplateId;
        public List<BehaviorConfiguration> behaviorConfigurations;
        public float timeUntilCorpseDisappears;
        public bool hasFootstepSounds;

        // Indexed animator access
        private static readonly int ForwardAnimationParam = Animator.StringToHash("Forward");
        private static readonly int TurnAnimationParam = Animator.StringToHash("Turn");
        private static readonly int MoveSet = Animator.StringToHash("moveSet");

        // Configuration classes
        [System.Serializable]
        public class BehaviorConfiguration
        {
            public UniversalAgentBehaviorType behavior;
            public int priority;
        }

        // Start is called before the first frame update
        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            interactionSystem = GetComponent<InteractionSystem>();
            aimIK = GetComponent<AimIK>();
            _appearance = GetComponent<UniversalAgentAppearance>();
            _damageTextObject = Resources.Load("DamageText") as GameObject;
            _originalMaterial = meshRenderer.material;
            _startingHeight = transform.position.y;

            puppetMaster.GetComponentsInChildren<AgentHitbox>().ToList()
                .ForEach(hitbox => hitbox.SetupHitbox(this, puppetMaster));
            puppetMaster.GetComponent<DamageRelay>().Setup(this);

            var agentTemplate = gameManager.agentManager.GetTemplate(agentTemplateId);
            SetupAgentBasedOnTemplate(agentTemplate);

            SetupBehaviors(agentTemplate);

            navMeshAgent.updatePosition = false;
        }

        private void SetupAgentBasedOnTemplate(AgentManager.AgentTemplate template)
        {
            navMeshAgent.speed = template.agentMaxSpeed;
            _appearance.SetAppearance(template.skin);
            animator.SetInteger(MoveSet, (int) template.moveSet);

            if (!template.hasWeapon)
            {
                weapon.gameObject.SetActive(false);
            }
            else
            {
                weapon.gameObject.SetActive(true);
            }
        }

        private void SetupBehaviors(AgentManager.AgentTemplate template)
        {
            behaviorConfigurations.ForEach(config =>
            {
                if (config.behavior == UniversalAgentBehaviorType.PickUpWeapon && template.hasWeapon)
                {
                    _behaviors.Add(new PickupWeaponBehavior(config, this));
                }

                if (config.behavior == UniversalAgentBehaviorType.RoamDungeon)
                {
                    _behaviors.Add(new RoamDungeonBehavior(config, this));
                }

                if (config.behavior == UniversalAgentBehaviorType.AttackPlayer)
                {
                    _behaviors.Add(new AttackPlayerBehavior(config, this));
                }

                if (config.behavior == UniversalAgentBehaviorType.Idle)
                {
                    _behaviors.Add(new IdleBehavior(config, this));
                }
            });
        }

        // Update is called once per frame
        void Update()
        {
            // Keep the agent disabled while the puppet is unbalanced.
            navMeshAgent.enabled = puppet.state == BehaviourPuppet.State.Puppet;

            if (navMeshAgent.enabled)
            {
                var executableBehaviorsByPriority = _behaviors.Where(behavior => behavior.CanBeExecuted()).ToList()
                    .OrderByDescending(behavior => behavior.GetPriority()).ToList();
                if (executableBehaviorsByPriority.Count > 0)
                {
                    executableBehaviorsByPriority[0].ExecuteUpdate();
                }

                var localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
                animator.SetFloat(ForwardAnimationParam, localVelocity.z);
                animator.SetFloat(TurnAnimationParam, localVelocity.x);

                // Fix positioning if character moves out of bounds, this can happen due to root-motion animations being
                // less accurate than what the navMesh agent predicts.
                Vector3 worldDeltaPosition = navMeshAgent.nextPosition - transform.position;
                if (worldDeltaPosition.magnitude > navMeshAgent.radius)
                {
                    transform.position = navMeshAgent.nextPosition - 0.9f * worldDeltaPosition;
                }
            }

            if (_healthPoints <= 0 && !_isDead)
            {
                Die();
            }

            DetectAndDestroyDefectiveAgent();
        }

        void OnAnimatorMove()
        {
            var position = animator.rootPosition;
            position.y = navMeshAgent.nextPosition.y;
            transform.position = position;
            navMeshAgent.nextPosition = transform.position;
        }

        public void Die()
        {
            _isDead = true;
            puppetMaster.Kill();
            _destructionListener.ForEach(listener => listener.ObjectDestroyed(gameObject));
            ++gameManager.statisticsManager.enemiesKilled;

            Destroy(rootObject, timeUntilCorpseDisappears);
        }

        private void DetectAndDestroyDefectiveAgent()
        {
            var heightDifference = _startingHeight - transform.position.y;
            if (heightDifference > 100)
            {
                Debug.Log("Destroying defective agent because height difference was >100");
                Destroy(rootObject);
            }
        }

        public void RegisterDestructionListener(IDestructionListener destructionListener)
        {
            _destructionListener.Add(destructionListener);
        }

        public void ApplyDirectDamage(float incomingDmg)
        {
            if (!_hitLocked)
            {
                _hitLocked = true;
                ApplyDamageAndUnlock((int) incomingDmg, transform.position);
            }
        }

        public void ApplyDamage(DamageObject damageObject, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            if (!_hitLocked)
            {
                _hitLocked = true;

                // Calculate damage
                var calculatedDmg = CalculateDamage(damageObject, relativeVelocityMagnitude);

                ApplyDamageAndUnlock(calculatedDmg, pointOfImpact);
            }
        }

        private void ApplyDamageAndUnlock(int dmg, Vector3 pointOfImpact)
        {
            audioSource.PlayOneShot(Helper.GETRandomFromList(dmgSounds));

            meshRenderer.material = dmgMaterial;

            // Damage Text
            var damageText = Instantiate(_damageTextObject, pointOfImpact, Quaternion.identity);
            damageText.GetComponent<DamageText>().SetDamageText(dmg);

            _healthPoints -= dmg;
            gameManager.statisticsManager.totalDamageDealt += dmg;
            Invoke(nameof(ResetHit), 1f);
        }

        private int CalculateDamage(DamageObject damageObject, float relativeVelocityMagnitude)
        {
            var baseDmgPoints = damageObject.GetDamagePoints();
            var criticalDamageRng = Random.Range(0, 3); //TODO: determine by weapon

            // Base 5 + 2-12 + 0-3 = 5 - 20
            return (int) (baseDmgPoints + relativeVelocityMagnitude + criticalDamageRng) *
                   gameManager.variabilityManager.player.damageDealtMultiplier;
        }

        private void ResetHit()
        {
            meshRenderer.material = _originalMaterial;
            _hitLocked = false;
        }

        public void Step()
        {
            if (hasFootstepSounds)
            {
                audioSourceFeet.PlayOneShot(Helper.GETRandomFromList(footStepSounds));
            }
        }
    }
}