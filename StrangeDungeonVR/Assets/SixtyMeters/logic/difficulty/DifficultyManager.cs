using System;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.difficulty
{
    /// <summary>
    /// The difficultyManager balances the enemies & traps vs. the players current stats. It is typically called at the
    /// end of every stage.
    /// </summary>
    public class DifficultyManager : MonoBehaviour
    {
        private GameManager _gameManager;

        // The game has 9 states of increasing difficulty
        public DifficultyStage currentStage;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// True if the player has fullfilled the requirements to progress to the next stage
        /// </summary>
        /// <returns></returns>
        public bool CanPlayerProgressToNextStage()
        {
            switch (currentStage)
            {
                case DifficultyStage.Stage1:
                    return _gameManager.player.Level > 3;
                case DifficultyStage.Stage2:
                    return _gameManager.player.Level > 6;
                case DifficultyStage.Stage3:
                    return _gameManager.player.Level > 9;
                case DifficultyStage.Stage4:
                //TODO: implement requirements for other stages
                case DifficultyStage.Stage5:
                case DifficultyStage.Stage6:
                case DifficultyStage.Stage7:
                case DifficultyStage.Stage8:
                case DifficultyStage.Stage9:
                    return false;
                default:
                    return false;
            }   
        }

        public void ProgressToNextStage()
        {
            switch (currentStage)
            {
                case DifficultyStage.Stage1:
                    SetCurrentStageToNext();
                    break;
                case DifficultyStage.Stage2:
                    SetCurrentStageToNext();
                    break;
                case DifficultyStage.Stage3:
                    SetCurrentStageToNext();
                    break;
                case DifficultyStage.Stage4:
                //TODO: implement requirements for other stages
                case DifficultyStage.Stage5:
                case DifficultyStage.Stage6:
                case DifficultyStage.Stage7:
                case DifficultyStage.Stage8:
                case DifficultyStage.Stage9:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetCurrentStageToNext()
        {
            currentStage += 1;
        }
    }
}