using System;
using HurricaneVR.Framework.Core.UI;
using SixtyMeters.logic.analytics;
using SixtyMeters.logic.generator;
using SixtyMeters.logic.item;
using SixtyMeters.logic.player;
using SixtyMeters.logic.variability;
using UnityEngine;

namespace SixtyMeters.logic.utilities
{
    /// <summary>
    /// Provides easy access to all manager type classes in the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public LevelManager levelManager;
        public DungeonGenerator dungeonGenerator;
        public VariabilityManager variabilityManager;
        public LootManager lootManager;
        public StatisticsManager statisticsManager;
        public HVRInputModule uiManager;

        [HideInInspector] public PlayerActor player;
        
        private void Start()
        {
            player = GameObject.Find("PlayerController").GetComponent<PlayerActor>();
        }
    }
}