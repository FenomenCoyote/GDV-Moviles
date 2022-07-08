using UnityEngine;
using UnityEngine.Advertisements;

namespace flow.Ads
{
    public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] string adUnitId = "Interstitial_Android";

        private Callback callback;

        bool alreadyAdded = false;

        public delegate void Callback(UnityAdsShowCompletionState completionState);

        /// <summary>
        /// Loads the intersticial ad if the ads manager is not initialized
        /// </summary>
        public void LoadAd()
        {
            if (!AdsManager.Instance.isInitialized)
                return;

            Advertisement.Load(adUnitId, this);
        }

        /// <summary>
        /// Shows the loaded content of th intersticial add
        /// </summary>
        /// <param name="callback">Callback method</param>
        public void ShowAd(Callback callback)
        {
            if (!AdsManager.Instance.isInitialized)
                return;

            this.callback = callback;

            //Important!. In the unity editor we pass the IUnityAdsShowListener once,in other case we pass it always.
            //This is for avoiding problems with the number of callback calls
#if UNITY_EDITOR
            if (!alreadyAdded)
            {
                Advertisement.Show(adUnitId, this);
                alreadyAdded = true;
            }
            else Advertisement.Show(adUnitId);
#else
			Advertisement.Show(adUnitId, this);
#endif
        }

        /// <summary>
        /// This method is called when the ad show is completed
        /// </summary>
        /// <param name="adUnitId">Ad id</param>
        /// <param name="showCompletionState">Completition state</param>
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(this.adUnitId) && callback != null)
            {
                callback(showCompletionState);
                callback = null;
            }
        }

        /// <summary>
        /// This method is called when the ad is loaded
        /// </summary>
        /// <param name="adUnitId"></param>
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
        }

        /// <summary>
        /// This method is called when ad failed to load
        /// </summary>
        /// <param name="adUnitId">Ad id</param>
        /// <param name="error">Type of error</param>
        /// <param name="message">Error mesage</param>
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        }

        /// <summary>
        /// This method is called when the ad failed to show
        /// </summary>
        /// <param name="adUnitId">Ad id</param>
        /// <param name="error">Type of error</param>
        /// <param name="message">Error mesage</param>
        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
            callback(UnityAdsShowCompletionState.UNKNOWN);
        }

        /// <summary>
        /// This method is called when the ad show starts
        /// </summary>
        /// <param name="adUnitId">Ad id</param>
        public void OnUnityAdsShowStart(string adUnitId) { }
        /// <summary>
        /// This method is called when we click on the ad
        /// </summary>
        /// <param name="adUnitId">Ad id</param>
        public void OnUnityAdsShowClick(string adUnitId) { }
    }
}