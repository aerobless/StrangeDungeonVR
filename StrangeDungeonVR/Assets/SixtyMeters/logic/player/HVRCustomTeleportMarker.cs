using HurricaneVR.Framework.Core.Player;
using UnityEngine;

namespace SixtyMeters.logic.player
{
    public class HVRCustomTeleportMarker : HVRTeleportMarkerBase
    {
        public GameObject arrow;
        public GameObject ring;
        public GameObject glow;

        public bool useTeleporterColors = true;
        public Color validColor;
        public Color invalidColor;

        private Material _ringMaterial;
        private Material _arrowMaterial;
        private Material _glowMaterial;
        private static readonly int TintColor = Shader.PropertyToID("_TintColor");


        private Color Color
        {
            get
            {
                if (useTeleporterColors)
                {
                    return IsTeleportValid ? Teleporter.ValidColor : Teleporter.InvalidColor;
                }

                return IsTeleportValid ? validColor : invalidColor;
            }
        }

        public override void Awake()
        {
            base.Awake();

            if (ring && ring.TryGetComponent(out MeshRenderer ringRenderer)) _ringMaterial = ringRenderer.material;
            if (arrow && arrow.TryGetComponent(out MeshRenderer arrowRenderer)) _arrowMaterial = arrowRenderer.material;
            if (glow && glow.TryGetComponent(out MeshRenderer glowRenderer)) _glowMaterial = glowRenderer.material;
        }


        protected override void OnActivated()
        {
            if (arrow) arrow.SetActive(true);
            if (ring) ring.SetActive(true);
            if (glow) glow.SetActive(true);
        }

        protected override void OnDeactivated()
        {
            if (arrow) arrow.SetActive(false);
            if (ring) ring.SetActive(false);
            if (glow) glow.SetActive(false);
        }

        public override void OnValidTeleportChanged(bool isTeleportValid)
        {
            base.OnValidTeleportChanged(isTeleportValid);

            UpdateMaterials();
        }

        protected virtual void UpdateMaterials()
        {
            if (_ringMaterial) _ringMaterial.color = Color;
            if (_arrowMaterial) _arrowMaterial.color = Color;
            if (_glowMaterial) _glowMaterial.SetColor(TintColor, Color);
        }
    }
}