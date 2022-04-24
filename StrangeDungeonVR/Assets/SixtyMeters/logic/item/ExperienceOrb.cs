using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.item
{
    public class ExperienceOrb : MonoBehaviour
    {
        
        // Internal components
        private GameManager _gameManager;
        
        // 
        
        private void Start()
        {
            _gameManager = GameManager.Instance;
        }

        private void Update()
        {
        
        }

        public void MoveToPlayer()
        {
            StartCoroutine(Helper.LerpPosition(transform,
                transform.position + new Vector3(0, 2, 0), 1f, () =>
                {
                    StartCoroutine(Helper.LerpWorldPosition(transform,
                        _gameManager.player.rightHand.transform, 2f,
                        () =>
                        {
                            _gameManager.player.CollectExp(10); //TODO: read for variability or agent
                            Destroy(gameObject);
                        }));
                }));
        }
    }
}