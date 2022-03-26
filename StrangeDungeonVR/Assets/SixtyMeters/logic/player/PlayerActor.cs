using System;
using System.Collections;
using HurricaneVR.Framework.Core.Grabbers;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.player
{
    public class PlayerActor : MonoBehaviour, IDamageable
    {
        // Components
        public GameObject head;
        public CanvasGroup dmgCanvas;
        public GameObject mainWeapon; //TODO: allow for a binding process later
        public HVRHandGrabber rightHand;

        // Internals components
        private GameManager _gameManager;

        // Settings
        public float damageCanvasInitialAlpha = 0.5f;
        public float damageCanvasDuration = 3f;

        // Internals
        private float _damageTaken;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
            if (_gameManager.variabilityManager)
            {
                ResetPlayer(false);
            }
        }

        /// <summary>
        /// Set all player stats to their base values
        /// </summary>
        private void ResetPlayer(bool afterDeath)
        {
            _damageTaken = 0; // Restores the player to full health
            if (afterDeath)
            {
                _gameManager.variabilityManager.RestoreInitialVariability();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (GetHealthPoints() <= 0)
            {
                ResetPlayer(true);
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
            _gameManager.statisticsManager.StopTracking();
            var respawnTile = _gameManager.dungeonGenerator.GenerateRespawnTile(gameObject.transform);
            respawnTile.EnableGraveyard();
            gameObject.transform.rotation = respawnTile.GetSpawnPoint().transform.rotation;
        }

        public void ApplyDirectDamage(float incomingDmg)
        {
            _damageTaken += incomingDmg * _gameManager.variabilityManager.player.damageTakenMultiplier;
            Debug.Log("Player took " + incomingDmg + " dmg and their HP is now " + GetHealthPoints());
            if (dmgCanvas)
            {
                dmgCanvas.alpha = damageCanvasInitialAlpha;
                StartCoroutine(LerpDamageScreen(dmgCanvas, 0, damageCanvasDuration));
            }
        }

        public void ApplyDamage(DamageObject damageObject, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            ApplyDirectDamage(damageObject.damagePerHit);
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

        /// <summary>
        /// Calculates the remaining HP of the player by detracting damage taken from their base health.
        /// This is done indirectly to allow for increasing the base health during the game.
        /// </summary>
        /// <returns>the HP of the player</returns>
        private float GetHealthPoints()
        {
            var playerBaseHealth = _gameManager.variabilityManager.player.baseHealth;
            return playerBaseHealth - _damageTaken;
        }

        /// <summary>
        /// Returns the current HP on a scale of 0..1. Used to display a health bar in the player HUD.
        /// </summary>
        /// <returns>the players current HP on a normalized scale</returns>
        public float GetNormalizedHp()
        {
            float playerBaseHealth = _gameManager.variabilityManager.player.baseHealth;
            float actualHealth = GetHealthPoints();
            return 1 / playerBaseHealth * actualHealth;
        }

        /// <summary>
        /// Heals the player for the specified amount of HP.
        /// </summary>
        /// <param name="hpToBeRestored"> the amount of HP to be restored</param>
        public void Heal(int hpToBeRestored)
        {
            _damageTaken -= hpToBeRestored;

            // Prevent overhealing.
            if (_damageTaken < 0)
            {
                _damageTaken = 0;
            }
        }
    }
}