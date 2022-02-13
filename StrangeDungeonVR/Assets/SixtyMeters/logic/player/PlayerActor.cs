using System.Collections;
using System.Collections.Generic;
using SixtyMeters.logic.fighting;
using UnityEngine;

namespace SixtyMeters.logic.player
{
    public class PlayerActor : MonoBehaviour, IDamageable
    {
        // Public
        public GameObject head;
        public CanvasGroup dmgCanvas;

        // Settings
        public float healthPoints = 100;
        public float damageCanvasInitialAlpha = 0.5f;
        public float damageCanvasDuration = 3f;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
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