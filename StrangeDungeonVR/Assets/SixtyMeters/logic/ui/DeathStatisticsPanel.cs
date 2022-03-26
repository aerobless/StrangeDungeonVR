using SixtyMeters.logic.analytics;
using TMPro;
using UnityEngine;

namespace SixtyMeters.logic.ui
{
    public class DeathStatisticsPanel : MonoBehaviour
    {
        // Internal components
        private StatisticsManager _statistics;

        // Header
        public TextMeshProUGUI timeInDungeon;
        public TextMeshProUGUI totalScore;

        // SubPanels
        public TextMeshProUGUI enemyStats;
        public TextMeshProUGUI dungeonStats;
        public TextMeshProUGUI treasureStats;

        // Start is called before the first frame update
        void Start()
        {
            _statistics = FindObjectOfType<StatisticsManager>();

            if (_statistics)
            {
                SetTimeInDungeon(_statistics.GetLastDungeonRunTimeInMinutes());
                SetTotalScore(_statistics.CalculateTotalScore());
                SetEnemyStats(_statistics.enemiesKilled, _statistics.totalDamageDealt, _statistics.bossBattles);
                SetDungeonStats(_statistics.roomsDiscovered, _statistics.totalTrapsInMap - _statistics.trapsTriggered,
                    _statistics.trapsTriggered);
                SetTreasureStats(_statistics.coinsCollected, _statistics.soulShardsFound, _statistics.soulShardsUsed,
                    _statistics.potionsUsed);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void SetTimeInDungeon(int timeInMinutes)
        {
            timeInDungeon.text = $"Time in dungeon: <color=#E09338>{timeInMinutes}min</color>";
        }

        private void SetTotalScore(int score)
        {
            totalScore.text = $"<color=#E09338>{score}</color>";
        }

        private void SetEnemyStats(int enemiesKilled, int damageDealt, int bossBattles)
        {
            enemyStats.text = $"Enemies killed: <color=#E09338>{enemiesKilled}</color>\n" +
                              $"Damage dealt: <color=#E09338>{damageDealt} HP</color>\n" +
                              $"Boss Battles: <color=#E09338>{bossBattles}</color>";
        }

        private void SetDungeonStats(int roomsDiscovered, int trapsAvoided, int trapsTriggered)
        {
            dungeonStats.text = $"Rooms discovered: <color=#E09338>{roomsDiscovered}</color>\n" +
                                $"Traps avoided: <color=#E09338>{trapsAvoided}</color>\n" +
                                $"Traps triggered: <color=#E09338>{trapsTriggered}</color>";
        }

        private void SetTreasureStats(int coinsCollected, int soulShardsFound, int soulShardsUsed, int potionsUsed)
        {
            treasureStats.text = $"Coins collected: <color=#E09338>{coinsCollected}</color>\n" +
                                 $"Soul shards found: <color=#E09338>{soulShardsFound}</color>\n" +
                                 $"Soul shards used: <color=#E09338>{soulShardsUsed}</color>\n" +
                                 $"Potions used: <color=#E09338>{potionsUsed}</color>";
        }
    }
}