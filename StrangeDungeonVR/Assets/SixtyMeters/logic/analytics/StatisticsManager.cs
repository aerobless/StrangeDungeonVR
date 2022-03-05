using System;
using UnityEngine;

namespace SixtyMeters.logic.analytics
{
    public class StatisticsManager : MonoBehaviour
    {
        private DateTime _dungeonRunStartTime;
        private DateTime _dungeonRunEndTime;
        private bool _isTracking = false;

        public int enemiesKilled = 0;
        public int totalDamageDealt = 0;
        public int bossBattles = 0;

        public int roomsGenerated = 0;
        public int roomsDiscovered = 0;
        public int trapTriggered = 0;
        public int totalTrapsInMap = 0;

        public int coinsCollected = 0;
        public int soulShardsFound = 0;
        public int soulShardsUsed = 0;
        public int potionsUsed = 0;


        // Start is called before the first frame update
        void Start()
        {
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
                _dungeonRunStartTime = DateTime.Now;
            }
        }

        public void StopTracking()
        {
            _dungeonRunEndTime = DateTime.Now;
            _isTracking = false;
        }

        public int GetLastDungeonRunTimeInMinutes()
        {
            return (_dungeonRunEndTime - _dungeonRunStartTime).Minutes;
        }

        public int CalculateTotalScore()
        {
            return enemiesKilled + trapTriggered + totalTrapsInMap + roomsGenerated + roomsDiscovered +
                   coinsCollected + soulShardsFound + soulShardsUsed + potionsUsed;
        }
    }
}