using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SixtyMeters.logic.ui
{
    public class NameTag : MonoBehaviour
    {
        // Components
        public Slider healthBar;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI nameText;

        public void Setup(int level, string npcName, float initialHealthPercentage)
        {
            levelText.SetText(level + "");
            nameText.SetText(npcName);
            healthBar.value = initialHealthPercentage;
        }

        public void SetHealthPercentage(float health, float timeForChange)
        {
            StartCoroutine(UpdateHealth(healthBar.value, health, timeForChange));
        }

        private IEnumerator UpdateHealth(float oldHealth, float newHealth, float timeForChange)
        {
            float time = 0;
            while (time < timeForChange)
            {
                healthBar.value = Mathf.Lerp(oldHealth, newHealth, time / timeForChange);
                time += Time.deltaTime;

                yield return null;
            }

            if (newHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}