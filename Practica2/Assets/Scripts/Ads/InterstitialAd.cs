using UnityEngine;
using UnityEngine.Advertisements;

namespace flow.Ads
{
    public class InterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] string _adUnitId = "Interstitial_Android";

        private Callback callback;

        bool alreadyAdded = false;

        public delegate void Callback(UnityAdsShowCompletionState completionState);

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            if (!AdsManager.Instance.isInitialized)
                return;

            Advertisement.Load(_adUnitId, this);
        }

        // Show the loaded content in the Ad Unit: 
        public void ShowAd(Callback callback)
        {
            if (!AdsManager.Instance.isInitialized)
                return;

            this.callback = callback;

#if UNITY_EDITOR
            if (!alreadyAdded)
            {
                Advertisement.Show(_adUnitId, this);
                alreadyAdded = true;
            }
            else Advertisement.Show(_adUnitId);
#else
			Advertisement.Show(_adUnitId, this);
#endif
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId))
            {
                callback(showCompletionState);
            }
        }

        // Implement Load Listener and Show Listener interface methods:  
        public void OnUnityAdsAdLoaded(string adUnitId)
        {

        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
            callback(UnityAdsShowCompletionState.UNKNOWN);
        }

        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
    }
}