using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;

namespace SixtyMeters.logic.social
{
    public class MetaPlatformManager : MonoBehaviour
    {
        public const string NoStringData = "no data";
        public const ulong NoUlongData = 1337;

        public ulong applicationScopedOculusId = NoUlongData;
        public string oculusUsernameId = NoStringData;

        void Awake()
        {
            try
            {
                Core.AsyncInitialize();
                Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
            }
            catch (UnityException e)
            {
                Debug.LogError("Platform failed to initialize due to exception.");
                Debug.LogException(e);
                // Immediately quit the application.
                UnityEngine.Application.Quit();
            }
        }


        // Called when the Oculus Platform completes the async entitlement check request and a result is available.
        void EntitlementCallback(Message msg)
        {
            if (msg.IsError) // User failed entitlement check
            {
                // Implements a default behavior for an entitlement check failure -- log the failure and exit the app.
                Debug.LogError("You are NOT entitled to use this app.");
                UnityEngine.Application.Quit();
            }
            else // User passed entitlement check
            {
                // Log the succeeded entitlement check for debugging.
                Debug.Log("You are entitled to use this app.");

                // Next get the identity of the user that launched the Application.
                Users.GetLoggedInUser().OnComplete(GetLoggedInUserCallback);
            }
        }

        void GetLoggedInUserCallback(Message<User> msg)
        {
            if (msg.IsError)
            {
                Debug.Log("Unable to get currently logged in user via Meta Platform");
                return;
            }

            applicationScopedOculusId = msg.Data.ID;
            oculusUsernameId = msg.Data.OculusID;
        }
    }
}