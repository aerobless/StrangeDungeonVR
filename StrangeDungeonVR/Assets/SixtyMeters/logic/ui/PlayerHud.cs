using System.Collections;
using SixtyMeters.logic.events;
using SixtyMeters.logic.utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SixtyMeters.logic.ui
{
    public class PlayerHud : MonoBehaviour
    {
        // Components
        public Slider healthBarUiSlider;
        public Image expCircle;
        public TextMeshProUGUI healthValueText;
        public TextMeshProUGUI levelValueText;
        public TextMeshProUGUI versionInfoText;

        // Internal components
        private GameManager _gameManager;

        // Settings
        private const float Tfc = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
            _gameManager.player.onHealthChanged.AddListener(HealthChangedEvent);
            _gameManager.player.onExpCollected.AddListener(ExpCollectedEvent);

            versionInfoText.text = "Undertown Alpha: " + Application.version;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void ExpCollectedEvent(ExpCollectedEvent expCollectedEvent)
        {
            StartCoroutine(UpdateExp(expCircle.fillAmount, expCollectedEvent.totalExpCollectedNormalized, Tfc));
        }

        private IEnumerator UpdateExp(float oldExp, float newExp, float timeForChange)
        {
            float time = 0;
            while (time < timeForChange)
            {
                expCircle.fillAmount = Mathf.Lerp(oldExp, newExp, time / timeForChange);
                time += Time.deltaTime;

                yield return null;
            }
        }

        private void HealthChangedEvent(HealthChangedEvent changedEvent)
        {
            StartCoroutine(UpdateHealth(healthBarUiSlider.value, changedEvent.NewHealthNormalized, Tfc));
            healthValueText.text = changedEvent.NewHealth + " / " + changedEvent.MaxHealth;
        }

        private IEnumerator UpdateHealth(float oldHealth, float newHealth, float timeForChange)
        {
            float time = 0;
            while (time < timeForChange)
            {
                healthBarUiSlider.value = Mathf.Lerp(oldHealth, newHealth, time / timeForChange);
                time += Time.deltaTime;

                yield return null;
            }
        }
    }
}