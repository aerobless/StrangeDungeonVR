using UnityEngine;
using Oculus.Platform;
using SixtyMeters.logic.ui;
using SixtyMeters.logic.utilities;

namespace SixtyMeters.logic.social
{
    public class LeaderboardManager : MonoBehaviour
    {
        private const string HighScoreLeaderboard = "strange-dungeon-highscore-leaderboard";

        // Components
        public GameObject leaderboardContent;
        public LeaderboardEntry entryTemplate;

        // Internal Components
        private GameManager _gameManager;

        // Start is called before the first frame update
        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            //TODO: add extra data, e.g. stage or level
            // The entry is only updated if its higher then a already recorded score
            Leaderboards.WriteEntry(HighScoreLeaderboard, _gameManager.statisticsManager.CalculateTotalScore())
                .OnComplete(_ => { UpdateLeaderboard(); });
        }

        private void UpdateLeaderboard()
        {
            ClearLeaderboard();
            Leaderboards.GetEntries(HighScoreLeaderboard, 10, LeaderboardFilterType.None,
                LeaderboardStartAt.CenteredOnViewerOrTop).OnComplete(callback =>
            {
                if (!callback.IsError)
                {
                    foreach (var entry in callback.GetLeaderboardEntryList())
                    {
                        var leaderboardEntry = Instantiate(entryTemplate, leaderboardContent.transform, false);
                        leaderboardEntry.UpdateEntry(entry.Rank, entry.User.DisplayName, entry.Score,
                            IsCurrentUser(entry));
                    }
                }
            });
        }

        private void ClearLeaderboard()
        {
            foreach (Transform child in leaderboardContent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private bool IsCurrentUser(Oculus.Platform.Models.LeaderboardEntry entry)
        {
            return entry.User.ID == _gameManager.platformManager.applicationScopedOculusId;
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}