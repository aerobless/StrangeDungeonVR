using System;
using Unity.VisualScripting;
using UnityEngine;

namespace SixtyMeters.logic.analytics
{
    public class StatisticsManager : MonoBehaviour
    {
        //Dungeon Run stats (resets for every run)
        private DateTime _dungeonRunStartTime;
        private DateTime _dungeonRunEndTime;
        private bool _isTracking = false;

        public int enemiesKilled = 0;
        public int totalDamageDealt = 0;
        public int bossBattles = 0;

        public int roomsGenerated = 0;
        public int roomsDiscovered = 0;
        public int trapsTriggered = 0;
        public int totalTrapsInMap = 0;

        public int coinsCollected = 0;
        public int soulShardsFound = 0;
        public int soulShardsUsed = 0;
        public int potionsUsed = 0;

        // Permanent Session Stats
        private DateTime _gameStartTime;
        public int sessionDeaths = 0;


        // Start is called before the first frame update
        void Start()
        {
            _gameStartTime = DateTime.Now;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void StartTrackingIfNotStartedYet()
        {
            if (!_isTracking)
            {
                _isTracking = true;
                Reset();
            }
        }

        public void StopTracking()
        {
            _dungeonRunEndTime = DateTime.Now;
            _isTracking = false;
        }

        public int GetLastDungeonRunTimeInMinutes()
        {
            if (_isTracking)
            {
                _dungeonRunEndTime = DateTime.Now;
            }

            return (_dungeonRunEndTime - _dungeonRunStartTime).Minutes;
        }

        public int TotalTimePlayedInCurrentSession()
        {
            return (DateTime.Now - _gameStartTime).Minutes;
        }

        public int CalculateTotalScore()
        {
            return enemiesKilled + trapsTriggered + totalTrapsInMap + roomsGenerated + roomsDiscovered +
                   coinsCollected + soulShardsFound + soulShardsUsed + potionsUsed;
        }

        public void Reset()
        {
            _dungeonRunStartTime = DateTime.Now;
            enemiesKilled = 0;
            totalDamageDealt = 0;
            bossBattles = 0;

            roomsGenerated = 0;
            roomsDiscovered = 0;
            trapsTriggered = 0;
            totalTrapsInMap = 0;

            coinsCollected = 0;
            soulShardsFound = 0;
            soulShardsUsed = 0;
            potionsUsed = 0;
        }
    }
}