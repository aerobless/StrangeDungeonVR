using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class Damageable : MonoBehaviour
    {
        public List<Hitbox> hitboxes;

        public SkinnedMeshRenderer meshRenderer;
        public Material dmgMaterial;

        private Material _originalMaterial;

        // Start is called before the first frame update
        void Start()
        {
            hitboxes.ForEach(hitbox => hitbox.SetDmgListener(this));
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