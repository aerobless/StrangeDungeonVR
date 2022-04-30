using TMPro;

namespace SixtyMeters.logic.generator
{
    public class StageCompleteDungeonTile : DungeonTile
    {
        public TextMeshProUGUI stageTitle;

        protected override void LateInit()
        {
            stageTitle.text = "Stage " + (int) GameManager.difficultyManager.currentStage + " Complete";
        }

        public void StageIncrement()
        {
            GameManager.difficultyManager.ProgressToNextStage();
        }
    }
}