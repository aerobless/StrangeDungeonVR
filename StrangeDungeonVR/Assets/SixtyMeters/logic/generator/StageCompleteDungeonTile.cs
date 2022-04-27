using TMPro;

namespace SixtyMeters.logic.generator
{
    public class StageCompleteDungeonTile : DungeonTile
    {
        public TextMeshProUGUI stageTitle;

        protected override void LateInit()
        {
            stageTitle.text = "Stage " + GameManager.statisticsManager.dungeonStage + " Complete";
        }

        public void StageIncrement()
        {
            //TODO: tbd
        }
    }
}