using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace SixtyMeters.logic.analytics
{
    public class AnalyticsManager : MonoBehaviour
    {
        // Store whether opt in consent is required, and what consent ID to use
        string _consentIdentifier;
        bool _consentHasBeenChecked;

        // Start is called before the first frame update
        async void Start()
        {
            try
            {
                var options = new InitializationOptions();

                //TODO: set environment depending on oculus release channel
                options.SetEnvironmentName("dev");
                await UnityServices.InitializeAsync(options);
                var consentIdentifiers = await Events.CheckForRequiredConsents();
                // TODO: build actual screen that asks user to opt-in
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
        }

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
                // Handle the exception by checking e.Reason
            }
        }

        public void onShowPrivacyPageButtonPressed()
        {
            // Open the Privacy Policy in the system's default browser
            Application.OpenURL(Events.PrivacyUrl);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}