using System.Collections;
using System.Collections.Generic;
using SixtyMeters.logic.dungeon;
using SixtyMeters.logic.fighting;
using UnityEngine;

namespace SixtyMeters.logic.player
{
    public class PlayerActor : MonoBehaviour, IDamageable
    {
        // Public
        public GameObject head;
        public CanvasGroup dmgCanvas;
        public List<GameObject> teleportWithPlayer;

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
                PlayerDeath();
            }
        }

        private void PlayerDeath()
        {
            Debug.Log("The player has died!");
            var deathTile = _dungeonGenerator.GenerateDeathTile();
            var routeToHeaven = deathTile.playerSpawnLocation.transform.position - gameObject.transform.position;
            gameObject.transform.position += routeToHeaven;
            teleportWithPlayer.ForEach(go => go.transform.position += routeToHeaven);
            healthPoints = 100;
        }

        public void ApplyDamage(float incomingDmg)
        {
            healthPoints -= incomingDmg;
            Debug.Log("Player took " + incomingDmg + " dmg and their HP is now " + healthPoints);
            if (dmgCanvas)
            {
                dmgCanvas.alpha = damageCanvasInitialAlpha;
                StartCoroutine(LerpDamageScreen(dmgCanvas, 0, damageCanvasDuration));
            }
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