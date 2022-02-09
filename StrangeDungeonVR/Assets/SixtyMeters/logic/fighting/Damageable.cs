using System.Collections.Generic;
using System.Linq;
using RootMotion.Dynamics;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class Damageable : MonoBehaviour
    {
        public GameObject hitboxParent;
        public PuppetMaster puppetMaster;

        public SkinnedMeshRenderer meshRenderer;
        public Material dmgMaterial;

        private Material _originalMaterial;

        // Start is called before the first frame update
        void Start()
        {
            hitboxParent.GetComponentsInChildren<Hitbox>().ToList().ForEach(hitbox => hitbox.SetupHitbox(this, puppetMaster));
            _originalMaterial = meshRenderer.material;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ApplyDamage(float incomingDmg)
        {
            Debug.Log("Sustained dmg: " + incomingDmg);
            meshRenderer.material = dmgMaterial;
            Invoke(nameof(ResetColor), 0.2f);
        }

        void ResetColor()
        {
            meshRenderer.material = _originalMaterial;
        }
    }
}