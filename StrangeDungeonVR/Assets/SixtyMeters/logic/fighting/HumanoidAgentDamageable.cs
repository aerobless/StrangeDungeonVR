using System.Collections.Generic;
using System.Linq;
using RootMotion.Dynamics;
using SixtyMeters.logic.ai;
using SixtyMeters.logic.utilities;
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

        // Internal settings
        private float _healthPoints;
        private bool _hitLocked;

        // Start is called before the first frame update
        void Start()
        {
            _humanoidAgent = GetComponent<HumanoidAgent>();
            _puppetMaster = _humanoidAgent.puppetMaster;
            _audioSource = _humanoidAgent.audioSource;

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

        public void ApplyDamage(float incomingDmg)
        {
            if (!_hitLocked)
            {
                _hitLocked = true;
                _healthPoints -= incomingDmg;
                
                _audioSource.PlayOneShot(Helper.GETRandomFromList(dmgSounds));

                meshRenderer.material = dmgMaterial;
                Debug.Log("Sustained dmg: " + incomingDmg);

                Invoke(nameof(ResetHit), 0.2f);
            }
        }

        private void ResetHit()
        {
            meshRenderer.material = _originalMaterial;
            _hitLocked = false;
        }
    }
    
}