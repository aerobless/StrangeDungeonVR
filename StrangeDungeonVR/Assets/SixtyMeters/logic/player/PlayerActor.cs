using System;
using System.Collections;
using HurricaneVR.Framework.Core.Grabbers;
using SixtyMeters.logic.events;
using SixtyMeters.logic.fighting;
using SixtyMeters.logic.utilities;
using UnityEngine;
using UnityEngine.Events;

namespace SixtyMeters.logic.player
{
    public class PlayerActor : MonoBehaviour, IDamageable
    {
        // Components
        public GameObject head;
        public CanvasGroup dmgCanvas;
        public GameObject mainWeapon; //TODO: allow for a binding process later
        public HVRHandGrabber rightHand;
        public GameObject xrRig;
        public GameObject xrTech;
        public CombatMarkerDisplay combatMarkerDisplay;
        public AudioSource playerDirectAudio;

        // Internals components
        private GameManager _gameManager;

        // Settings
        public float damageCanvasInitialAlpha = 0.5f;
        public float damageCanvasDuration = 3f;

        //TODO: increase dynamically via variability
        private const float ExpNeededForNextLevel = 100;

        // Internals
        private float _damageTaken;
        private float _expCollected;
        private int _level = 1;

        // Events
        public UnityEvent<HealthChangedEvent> onHealthChanged = new();
        public UnityEvent<ExpCollectedEvent> onExpCollected = new();
        public UnityEvent<int> onLevelUp = new();


        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
            if (_gameManager.variabilityManager)
            {
                ResetPlayer(false);
            }
        }

        /// <summary>
        /// Set all player stats to their base valuesF
        /// </summary>
        private void ResetPlayer(bool afterDeath)
        {
            HealthChange(_damageTaken * -1); // Restores the player to full health
            CollectExp(_expCollected * -1); // Restore collected exp to zero
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

        //TODO: remove me if no longer needed
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
            _gameManager.platformManager.UpdatePlayerScore();
            _gameManager.analyticsManager.FlushEvents();
        }

        public void ApplyDirectDamage(float incomingDmg)
        {
            var damageAmount = incomingDmg * _gameManager.variabilityManager.player.damageTakenMultiplier;

            // Apply dmg
            HealthChange(damageAmount);

            Debug.Log("Player took " + incomingDmg + " dmg and their HP is now " + GetHealthPoints());
            if (dmgCanvas)
            {
                dmgCanvas.alpha = damageCanvasInitialAlpha;
                StartCoroutine(LerpDamageScreen(dmgCanvas, 0, damageCanvasDuration));
            }
        }

        private void HealthChange(float dmgTakenAmountPerEvent)
        {
            var playerBaseHealth = _gameManager.variabilityManager.player.baseHealth;
            _damageTaken += dmgTakenAmountPerEvent;

            // Prevent overhealing.
            if (_damageTaken < 0)
            {
                _damageTaken = 0;
            }

            onHealthChanged.Invoke(new HealthChangedEvent(dmgTakenAmountPerEvent, GetHealthPoints(), GetNormalizedHp(),
                playerBaseHealth));
        }

        public void ApplyDamage(float incomingDmg, float relativeVelocityMagnitude, Vector3 pointOfImpact)
        {
            ApplyDirectDamage(incomingDmg);
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
        public float GetHealthPoints()
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
            var actualHealth = GetHealthPoints();
            return 1 / playerBaseHealth * actualHealth;
        }

        /// <summary>
        /// Heals the player for the specified amount of HP.
        /// </summary>
        /// <param name="hpToBeRestored"> the amount of HP to be restored</param>
        public void Heal(int hpToBeRestored)
        {
            HealthChange(hpToBeRestored * -1);
        }

        public void CollectExp(float amount)
        {
            _expCollected += amount;
            if (_expCollected >= ExpNeededForNextLevel)
            {
                _expCollected -= ExpNeededForNextLevel;
                _level++;
                onLevelUp.Invoke(_level);
            }

            var normalizedExp = 1 / ExpNeededForNextLevel * _expCollected;
            onExpCollected.Invoke(new ExpCollectedEvent(normalizedExp));
        }

        /// <summary>
        /// Attaches the players XR-Rig to the given gameObject. This applies all forces that interact with the given
        /// game object to the player as well. E.g. useful for moving elevators, platforms etc.
        /// </summary>
        /// <param name="newAttachment">the gameObject to which the player should be attached temporarily</param>
        public void AttachPlayerToGameObject(GameObject newAttachment)
        {
            xrRig.transform.parent = newAttachment.transform;
        }

        /// <summary>
        /// Re-Attaches the players XR-Rig to the XR-Tech gameObject. This restores the default state in which no
        /// special forces have a direct impact upon the player. Call after the player moves off a moving platform or
        /// elevator etc.
        /// </summary>
        public void RestorePlayerAttachmentToDefault()
        {
            xrRig.transform.parent = xrTech.transform;
        }

        public bool IsInRange(Transform caller, float range)
        {
            return Vector3.Distance(caller.position, gameObject.transform.position) <= range;
        }

        public int Level => _level;
    }
}