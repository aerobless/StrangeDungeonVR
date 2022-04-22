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

        // Internals
        private Canvas _canvas;
        private Transform _playerCameraTransform;

        private void Start()
        {
            _canvas = GetComponent<Canvas>();
            _playerCameraTransform = Camera.main.transform;
        }

        public void Setup(int level, string npcName, float initialHealthPercentage)
        {
            levelText.SetText(level + "");
            nameText.SetText(npcName);
            healthBar.value = initialHealthPercentage;
            gameObject.SetActive(false);
        }

        public void SetHealthPercentage(float health, float timeForChange)
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(UpdateHealth(healthBar.value, health, timeForChange));
            }
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

        private void LateUpdate()
        {
            RotateCanvasToFacePlayer();
        }

        private void RotateCanvasToFacePlayer()
        {
            _canvas.transform.rotation =
                Quaternion.LookRotation(_canvas.transform.position - _playerCameraTransform.position);
        }
    }
}