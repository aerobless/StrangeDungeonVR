using System;
using System.Collections.Generic;
using System.Linq;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
using SixtyMeters.logic.ai.appearance;
using SixtyMeters.logic.ai.behaviors;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.interfaces.lifecycle;
using SixtyMeters.logic.utilities;
using UnityEngine;
using UnityEngine.AI;
using static SixtyMeters.logic.fighting.CombatMarkerMove;
using AnimationInfo = SixtyMeters.logic.ai.appearance.AnimationInfo;
using Random = UnityEngine.Random;

namespace SixtyMeters.logic.ai
{
    public class UniversalAgent : MonoBehaviour, ITrackedLifecycle, IDamageable, IEnemy
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
        public List<AudioClip> battleCry;

        // Internal components
        [HideInInspector] public GameManager gameManager;

        [HideInInspector] public NavMeshAgent navMeshAgent;

        [HideInInspector] public Animator animator;

        [HideInInspector] public InteractionSystem interactionSystem;

        [HideInInspector] public AimIK aimIK;

        [HideInInspector] public AgentManager.AgentTemplate template;

        private GameObject _damageTextObject;
        private UniversalAgentAppearance _appearance;

        // Internals
        private readonly List<UniversalAgentBehavior> _behaviors = new();
        private readonly List<IDestructionListener> _destructionListener = new();
        private List<AnimationInfo> _dmgReactionAnimations = new();
        private Material _originalMaterial;
        private float _healthPoints;
        private bool _hitLocked;
        private bool _isDead;
        private float _startingHeight; // Used to determine if agent is falling out of map
        private bool _executeBehavior = true;

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
        [Serializable]
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

            _dmgReactionAnimations.Add(AnimationIndex.ImpactHeadHit);
            _dmgReactionAnimations.Add(AnimationIndex.ImpactCenterSwordHit);

            puppetMaster.GetComponentsInChildren<PhysicalAgentHitbox>().ToList()
                .ForEach(hitbox => hitbox.SetupHitbox(this, puppetMaster));
            puppetMaster.GetComponent<DamageRelay>().Setup(this);

            template = gameManager.agentManager.GetTemplate(agentTemplateId);
            SetupAgentBasedOnTemplate(template);

            SetupBehaviors(template);

            navMeshAgent.updatePosition = false;
        }

        private void SetupAgentBasedOnTemplate(AgentManager.AgentTemplate template)
        {
            navMeshAgent.speed = template.agentRoamMaxMovementSpeed;
            _appearance.SetAppearance(template.skin);
            animator.SetInteger(MoveSet, (int) template.moveSet);
            _healthPoints = template.healthPoints;

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

                if (config.behavior == UniversalAgentBehaviorType.UnarmedCombatBehavior && !template.hasWeapon)
                {
                    _behaviors.Add(new UnarmedCombatBehavior(config, this));
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
                if (_executeBehavior)
                {
                    var executableBehaviorsByPriority = _behaviors.Where(behavior => behavior.CanBeExecuted()).ToList()
                        .OrderByDescending(behavior => behavior.GetPriority()).ToList();
                    if (executableBehaviorsByPriority.Count > 0)
                    {
                        executableBehaviorsByPriority[0].ExecuteUpdate();
                    }
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

        public void ApplyDamage(float incomingDmg, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            if (!_hitLocked)
            {
                _hitLocked = true;

                // Calculate damage
                var calculatedDmg = CalculateDamage(incomingDmg, relativeVelocityMagnitude);

                ApplyDamageAndUnlock(calculatedDmg, pointOfImpact);
            }
        }

        private void ApplyDamageAndUnlock(int dmg, Vector3 pointOfImpact)
        {
            //audioSource.PlayOneShot(Helper.GETRandomFromList(dmgSounds));

            animator.SetTrigger(Helper.GETRandomFromList(_dmgReactionAnimations).Id);

            meshRenderer.material = dmgMaterial;

            // Damage Text
            var damageText = Instantiate(_damageTextObject, pointOfImpact, Quaternion.identity);
            damageText.GetComponent<DamageText>().SetDamageText(dmg);

            _healthPoints -= dmg;
            gameManager.statisticsManager.totalDamageDealt += dmg;
            Invoke(nameof(ResetHit), 1f);
        }

        private int CalculateDamage(float incomingDmg, float relativeVelocityMagnitude)
        {
            var criticalDamageRng = Random.Range(0, 3); //TODO: determine by weapon

            // Base 5 + 2-12 + 0-3 = 5 - 20
            return (int) (incomingDmg + relativeVelocityMagnitude + criticalDamageRng) *
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

        internal void LockBehaviorExecution(float timeToLock)
        {
            _executeBehavior = false;
            StartCoroutine(Helper.Wait(timeToLock, () => { _executeBehavior = true; }));
        }

        public void AnimationEvent(AnimationEvent animationEvent)
        {
            switch ((AnimationEventType) animationEvent.intParameter)
            {
                case AnimationEventType.BattleCry:
                    audioSource.PlayOneShot(Helper.GETRandomFromList(battleCry));
                    break;
                case AnimationEventType.BlockRight:
                    gameManager.player.combatMarkerDisplay.ActivateCombatMove(SingleBlockRightDefense, template.damagePerHit, this);
                    break;
                case AnimationEventType.BlockTop:
                    gameManager.player.combatMarkerDisplay.ActivateCombatMove(SingleBlockTopDefense,  template.damagePerHit, this);
                    break;
                case AnimationEventType.BlockLeft:
                    gameManager.player.combatMarkerDisplay.ActivateCombatMove(SingleBlockLeftDefense,  template.damagePerHit, this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsAlive()
        {
            return _healthPoints > 0;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}