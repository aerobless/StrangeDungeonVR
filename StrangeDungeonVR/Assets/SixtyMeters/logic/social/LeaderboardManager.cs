using UnityEngine;
using Oculus.Platform;

namespace SixtyMeters.logic.social
{
    public class LeaderboardManager : MonoBehaviour
    {
        private const string HighScoreLeaderboard = "strange-dungeon-highscore-leaderboard";


        // Start is called before the first frame update
        void Start()
        {
            Leaderboards.WriteEntry(HighScoreLeaderboard, 20);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}