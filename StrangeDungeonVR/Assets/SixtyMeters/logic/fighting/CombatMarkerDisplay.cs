using System.Collections.Generic;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.fighting
{
    public class CombatMarkerDisplay : MonoBehaviour
    {
        [System.Serializable]
        public class CombatMarkerMoveConfig
        {
            public CombatMarkerMove move;
            public List<CombatMarker> markers = new();
        }

        public List<CombatMarkerMoveConfig> combatMarkerConfig;

        // Internal components
        private GameManager _gameManager;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ActivateCombatMove(CombatMarkerMove move, float baseDamageOnFailure, IEnemy target)
        {
            var config = combatMarkerConfig.Find(conf => conf.move.Equals(move));
            var ttr = _gameManager.variabilityManager.player.timeToRespondToCombatMarker;
            //TODO: logic to handle multiple combat markers
            config.markers[0].Activate(ttr, baseDamageOnFailure, target);
        }
    }
}