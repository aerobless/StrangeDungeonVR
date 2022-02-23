using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Grabbers;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.generator;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.player
{
    public class PlayerActor : MonoBehaviour, IDamageable
    {
        // Public
        public GameObject head;
        public CanvasGroup dmgCanvas;
        public List<GameObject> teleportWithPlayer;
        public GameObject mainWeapon; //TODO: allow for a binding process later
        public HVRHandGrabber rightHand;

        // Settings
        public float healthPoints = 100;
        public float damageCanvasInitialAlpha = 0.5f;
        public float damageCanvasDuration = 3f;

        // Internals set up at start
        private DungeonGenerator _dungeonGenerator;

        // Start is called before the first frame update
        void Start()
        {
            _dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<DungeonGenerator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (healthPoints <= 0)
            {
                healthPoints = 100;
                PlayerDeath();
            }
        }

        public void SummonMainWeapon()
        {
            // if weird bugs appear - the players sword needs to be attached to his gameobject otherwise the local lerp won't work
            StartCoroutine(Helper.LerpPosition(mainWeapon.transform, rightHand.transform, 1,
                GrabWeapon(rightHand, mainWeapon)));
            //rightHand.Grab(mainWeapon.GetComponent<HVRGrabbable>(), HVRGrabTrigger.Toggle);
        }

        private static Action GrabWeapon(HVRHandGrabber hand, GameObject weapon)
        {
            return () => { }; //hand.Grab(weapon.GetComponent<HVRGrabbable>(), HVRGrabTrigger.Active); };
        }

        private void PlayerDeath()
        {
            Debug.Log("The player has died!");
            var respawnTile = _dungeonGenerator.GenerateRespawnTile(gameObject.transform);
            respawnTile.EnableGraveyard();
            gameObject.transform.rotation = respawnTile.GetSpawnPoint().transform.rotation;
            healthPoints = 100;
        }

        public void ApplyDirectDamage(float incomingDmg)
        {
            healthPoints -= incomingDmg;
            Debug.Log("Player took " + incomingDmg + " dmg and their HP is now " + healthPoints);
            if (dmgCanvas)
            {
                dmgCanvas.alpha = damageCanvasInitialAlpha;
                StartCoroutine(LerpDamageScreen(dmgCanvas, 0, damageCanvasDuration));
            }
        }

        public void ApplyDamage(DamageObject damageObject, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            throw new System.NotImplementedException();
        }

        private static IEnumerator LerpDamageScreen(CanvasGroup canvas, float targetValue, float duration)
        {
            float time = 0;
            var startValue = canvas.alpha;
            while (time < duration)
            {
                canvas.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            canvas.alpha = targetValue;
        }
    }
}