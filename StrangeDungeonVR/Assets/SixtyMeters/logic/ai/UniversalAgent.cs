using System.Collections.Generic;
using System.Linq;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
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
        public Material dmgMaterial;
        public List<AudioClip> dmgSounds;

        // Internal components
        [HideInInspector] public GameManager gameManager;

        [HideInInspector] public NavMeshAgent navMeshAgent;

        [HideInInspector] public Animator animator;

        [HideInInspector] public InteractionSystem interactionSystem;

        [HideInInspector] public AimIK aimIK;

        private GameObject _damageTextObject;

        // Internals
        private readonly List<UniversalAgentBehavior> _behaviors = new();
        private readonly List<IDestructionListener> _destructionListener = new();
        private Material _originalMaterial;
        private float _healthPoints = 100; //TODO: read from variability manager
        private bool _hitLocked;
        private bool _isDead;
        private float _startingHeight; // Used to determine if agent is falling out of map

        // Settings
        public List<BehaviorConfiguration> behaviorConfigurations;
        public float timeUntilCorpseDisappears;

        // Indexed animator access
        private static readonly int MoveForwardAnimation = Animator.StringToHash("Forward");

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
            _damageTextObject = Resources.Load("DamageText") as GameObject;
            _originalMaterial = meshRenderer.material;
            _startingHeight = transform.position.y;

            puppetMaster.GetComponentsInChildren<AgentHitbox>().ToList()
                .ForEach(hitbox => hitbox.SetupHitbox(this, puppetMaster));
            puppetMaster.GetComponent<DamageRelay>().Setup(this);

            SetupBehaviors();
        }

        private void SetupBehaviors()
        {
            behaviorConfigurations.ForEach(config =>
            {
                if (config.behavior == UniversalAgentBehaviorType.PickUpWeapon)
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

                animator.SetFloat(MoveForwardAnimation, navMeshAgent.velocity.magnitude * 0.25f);
            }

            if (_healthPoints <= 0 && !_isDead)
            {
                Die();
            }

            DetectAndDestroyDefectiveAgent();
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
            Debug.Log("Sustained dmg: " + dmg);

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
    }
}