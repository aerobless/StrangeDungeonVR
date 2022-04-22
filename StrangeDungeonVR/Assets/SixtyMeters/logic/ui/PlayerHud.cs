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
        public Image experienceBarSlider;
        public TextMeshProUGUI healthValueText;
        public TextMeshProUGUI levelValueText;
        public TextMeshProUGUI versionInfoText;

        // Internal components
        private GameManager _gameManager;

        // Settings
        private const float TimeForChange = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
            _gameManager.player.onHealthChanged.AddListener(HealthChangedEvent);
            versionInfoText.text = "Undertown Alpha: " + Application.version;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void HealthChangedEvent(HealthChangedEvent changedEvent)
        {
            StartCoroutine(UpdateHealth(healthBarUiSlider.value, changedEvent.NewHealthNormalized, TimeForChange));
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

            if (newHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}