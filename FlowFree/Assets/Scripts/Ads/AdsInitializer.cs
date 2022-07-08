using UnityEngine;
using UnityEngine.Advertisements;

namespace flow.Ads
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        [SerializeField] string androidGameId;
        [SerializeField] bool testMode = true;

        private InitializedCallback initializedCallback;

        public delegate void InitializedCallback();

        /// <summary>
        /// Initializes the advertisement system using the game id (android or ios)
        /// </summary>
        /// <param name="initializedCallback">Initialization callback</param>
        public void InitializeAds(InitializedCallback initializedCallback)
        {
            this.initializedCallback = initializedCallback;
            Advertisement.Initialize(androidGameId, testMode, this);
        }

        /// <summary>
        /// This method is called when the initialization is completed
        /// </summary>
        public void OnInitializationComplete()
        {
            initializedCallback();
        }

        /// <summary>
        /// This method is called when the initialization filed
        /// </summary>
        /// <param name="error">Error type</param>
        /// <param name="message">Error message</param>
        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}