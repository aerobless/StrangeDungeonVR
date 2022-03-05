using System.Collections.Generic;
using System.Linq;
using RootMotion.Dynamics;
using SixtyMeters.logic.ai;
using SixtyMeters.logic.utilities;
using SixtyMeters.logic.variability;
using Unity.VisualScripting;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class HumanoidAgentDamageable : MonoBehaviour, IDamageable
    {
        // Public 
        public SkinnedMeshRenderer meshRenderer;
        public Material dmgMaterial;
        public List<AudioClip> dmgSounds;

        private Material _originalMaterial;

        // Internal components
        private HumanoidAgent _humanoidAgent;
        private PuppetMaster _puppetMaster;
        private AudioSource _audioSource;
        private GameObject _damageTextObject;
        private VariabilityManager _variabilityManager;

        // Internal settings
        private float _healthPoints;
        private bool _hitLocked;

        // Start is called before the first frame update
        void Start()
        {
            _humanoidAgent = GetComponent<HumanoidAgent>();
            _puppetMaster = _humanoidAgent.puppetMaster;
            _audioSource = _humanoidAgent.audioSource;
            _damageTextObject = Resources.Load("DamageText") as GameObject;
            _variabilityManager = FindObjectOfType<VariabilityManager>();

            _puppetMaster.GetComponentsInChildren<AgentHitbox>().ToList()
                .ForEach(hitbox => hitbox.SetupHitbox(this, _puppetMaster));
            _originalMaterial = meshRenderer.material;

            // Settings Setup
            _healthPoints = _humanoidAgent.healthPoints;
        }

        // Update is called once per frame
        void Update()
        {
            if (_healthPoints <= 0)
            {
                _humanoidAgent.Die();
            }
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
            _audioSource.PlayOneShot(Helper.GETRandomFromList(dmgSounds));

            meshRenderer.material = dmgMaterial;

            // Damage Text
            var damageText = Instantiate(_damageTextObject, pointOfImpact, Quaternion.identity);
            damageText.GetComponent<DamageText>().SetDamageText(dmg);
            Debug.Log("Sustained dmg: " + dmg);

            _healthPoints -= dmg;
            Invoke(nameof(ResetHit), 1f);
        }

        private int CalculateDamage(DamageObject damageObject, float relativeVelocityMagnitude)
        {
            var baseDmgPoints = damageObject.GetDamagePoints();
            var criticalDamageRng = Random.Range(0, 3); //TODO: determine by weapon

            // Base 5 + 2-12 + 0-3 = 5 - 20
            return (int) (baseDmgPoints + relativeVelocityMagnitude + criticalDamageRng) *
                   _variabilityManager.player.damageDealtMultiplier;
        }

        private void ResetHit()
        {
            meshRenderer.material = _originalMaterial;
            _hitLocked = false;
        }
    }
}