using TMPro;
using UnityEngine;

namespace SixtyMeters.logic.ui
{
    public class LeaderboardEntry : MonoBehaviour
    {
        public TMP_Text rankTMP;
        public TMP_Text userIdTMP;
        public TMP_Text totalPointScoreTMP;
        public RectTransform myBackgroundRect;

        public void UpdateEntry(int rank, string userId, long totalPointScore, bool ownEntry)
        {
            rankTMP.SetText(rank + ".");
            userIdTMP.SetText(userId);
            totalPointScoreTMP.SetText(totalPointScore + " TPS");
            myBackgroundRect.gameObject.SetActive(ownEntry);
        }
    }
}