using SixtyMeters.logic.utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SixtyMeters.logic.ui
{
    public class PlayerHud : MonoBehaviour
    {
        public Slider healthBarUiSlider;

        // Internal components
        private GameManager _gameManager;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {
            healthBarUiSlider.value = _gameManager.player.GetNormalizedHp();
        }
    }
}