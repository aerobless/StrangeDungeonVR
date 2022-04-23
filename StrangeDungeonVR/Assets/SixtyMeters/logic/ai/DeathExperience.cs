using SixtyMeters.logic.item;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.ai
{
    public class DeathExperience : MonoBehaviour
    {
        // Components
        public ExperienceOrb deathOrb;

        // Internal components
        private GameManager _gameManager;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
            GetComponent<UniversalAgent>().onDeath.AddListener(DeathEvent);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void DeathEvent()
        {
            if (_gameManager.player.IsInRange(transform, 10f))
            {
                var enemyTransform = gameObject.transform;
                var spawnedOrb = Instantiate(deathOrb, enemyTransform.position, enemyTransform.rotation);
                spawnedOrb.MoveToPlayer();
            }
        }
    }
}