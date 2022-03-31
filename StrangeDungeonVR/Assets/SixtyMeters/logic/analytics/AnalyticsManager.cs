using System.Collections.Generic;
using SixtyMeters.logic.utilities;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace SixtyMeters.logic.analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        // Internal components
        private GameManager _gameManager;

        // Internals
        private string _consentIdentifier;
        private bool _consentHasBeenChecked;
        private const string DungeonProgressEvent = "dungeonProgress";

        // Start is called before the first frame update
        async void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();

            try
            {
                var options = new InitializationOptions();

                //TODO: set environment depending on oculus release channel
                options.SetEnvironmentName("dev");
                await UnityServices.InitializeAsync(options);
                var consentIdentifiers = await Events.CheckForRequiredConsents();

                // I'm a bit unsure about this, but I haven't seen any games on the oculus store ask for consent explicitly.
                // My current assumption is that since you have to provide a privacy policy on the oculus store that details handling
                // of user data, this already includes "consent" when a user installs an app. 
                consentIdentifiers.ForEach(id =>
                {
                    Events.ProvideOptInConsent(id, true);
                    Debug.Log("Consent given for " + id);
                });
                Debug.Log("Analytics enabled (" + consentIdentifiers.Count + " consent identifiers)");
            }
            catch (ConsentCheckException e)
            {
                // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately
                Debug.Log("Unable to enable analytics " + e.Reason);
            }

            // Update the data for the dungeon progress event every 10 seconds
            // The data is only uploaded every minute though
            InvokeRepeating(nameof(UpdateDungeonProgressEvent), 100, 100);
        }

        // TODO: to be compliant with GDPR I will probably have to offer an opt-out at least.. 
        public void OptOut()
        {
            try
            {
                if (!_consentHasBeenChecked)
                {
                    // Show a GDPR/COPPA/other opt-out consent flow
                    // If a user opts out
                    Events.OptOut();
                }

                // Record that we have checked a user's consent, so we don't repeat the flow unnecessarily. 
                // In a real game, use PlayerPrefs or an equivalent to persist this state between sessions
                _consentHasBeenChecked = true;
            }
            catch (ConsentCheckException e)
            {
                Debug.Log(e.Reason);
                // Handle the exception by checking e.Reason
            }
        }

        // TODO: probably not needed since this is included in the oculus store privacy policy
        public void OnShowPrivacyPageButtonPressed()
        {
            // Open the Privacy Policy in the system's default browser
            Application.OpenURL(Events.PrivacyUrl);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void UpdateDungeonProgressEvent()
        {
            var dungeonProgressEventParameters = new Dictionary<string, object>
            {
                // Live info
                {"playerHp", _gameManager.player.GetHealthPoints()},
                {"playerMaxHp", _gameManager.variabilityManager.player.baseHealth},
                {"playerNormalizedHp", _gameManager.player.GetNormalizedHp()},

                // Current run stats
                {"statisticsDungeonRunTime", _gameManager.statisticsManager.GetLastDungeonRunTimeInMinutes()},
                {"statisticsTotalScore", _gameManager.statisticsManager.CalculateTotalScore()},
                {"statisticsBossBattles", _gameManager.statisticsManager.bossBattles},
                {"statisticsCoinsCollected", _gameManager.statisticsManager.coinsCollected},
                {"statisticsEnemiesKilled", _gameManager.statisticsManager.enemiesKilled},
                {"statisticsPotionsUsed", _gameManager.statisticsManager.potionsUsed},
                {"statisticsRoomsDiscovered", _gameManager.statisticsManager.roomsDiscovered},
                {"statisticsRoomsGenerated", _gameManager.statisticsManager.roomsGenerated},
                {"statisticsTrapsTriggered", _gameManager.statisticsManager.trapsTriggered},
                {"statisticsSoulShardsFound", _gameManager.statisticsManager.soulShardsFound},
                {"statisticsSoulShardsUsed", _gameManager.statisticsManager.soulShardsUsed},
                {"statisticsTotalDamageDealt", _gameManager.statisticsManager.totalDamageDealt},
                {"statisticsTotalTrapsInMap", _gameManager.statisticsManager.totalTrapsInMap},

                // Session stats
                {"sessionPlaytime", _gameManager.statisticsManager.TotalTimePlayedInCurrentSession()},
                {"sessionTotalDeaths", _gameManager.statisticsManager.sessionDeaths},
            };

            // The event will get queued up and sent every minute
            Events.CustomData(DungeonProgressEvent, dungeonProgressEventParameters);
        }

        /// <summary>
        /// Normally events are queued up and sent every minute. Calling this sends them immediately.
        /// </summary>
        public void FlushEvents()
        {
            UpdateDungeonProgressEvent();
            Events.Flush();
        }
    }
}