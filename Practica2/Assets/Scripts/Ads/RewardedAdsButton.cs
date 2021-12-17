using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace flow.Ads
{
	public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
	{
		Button showAdButton;
		[SerializeField] string adUnitId = "Rewarded_Android";
		[SerializeField] LevelManager levelManager;

		bool alreadyAdded = false;

		void Awake()
		{
			showAdButton = GetComponent<Button>();
			//Disable button until ad is ready to show
			showAdButton.interactable = false;
		}

		void Start()
        {
			if (AdsManager.Instance.isInitialized)
				LoadAd();
			else
				AdsManager.Instance.waitForInitialization(LoadAd);
        }

		// Load content to the Ad Unit:
		public void LoadAd()
		{
			if (!AdsManager.Instance.isInitialized)
				return;

			Advertisement.Load(adUnitId, this);
		}

		// If the ad successfully loads, add a listener to the button and enable it:
		public void OnUnityAdsAdLoaded(string adUnitId)
		{
			if (adUnitId.Equals(this.adUnitId))
			{
                // Configure the button to call the ShowAd() method when clicked
                showAdButton.onClick.AddListener(this.ShowAd);
                // Enable the button for users to click:
                showAdButton.interactable = true;
			}
		}

		// Implement a method to execute when the user clicks the button.
		public void ShowAd()
		{
			if (!AdsManager.Instance.isInitialized)
				return;

			//We disable the button 
			showAdButton.interactable = false;

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
		/// This method is called when the ad show is completed. Here we add a hint when the user cpmpletes the ad
		/// </summary>
		/// <param name="adUnitId">Ad id</param>
		/// <param name="showCompletionState">Completition state</param>
		public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
		{
			if (adUnitId.Equals(this.adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
			{
                Debug.Log("Unity Ads Rewarded Ad Completed");
                levelManager.addHint();

				//We show the banner ad
				Advertisement.Banner.Show("Banner_Android"); 

                //We load another ad
                Advertisement.Load(this.adUnitId, this);
			}
		}

		/// <summary>
		/// This method is called when ad failed to load
		/// </summary>
		/// <param name="adUnitId">Ad id</param>
		/// <param name="error">Type of error</param>
		/// <param name="message">Error mesage</param>
		public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
		{
			Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
		}

		/// <summary>
		/// This method is called when ad failed to show
		/// </summary>
		/// <param name="adUnitId">Ad id</param>
		/// <param name="error">Type of error</param>
		/// <param name="message">Error mesage</param>
		public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
		{
			Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
		}

		/// <summary>
		/// This method is called when the ad show starts. Here we hide banned ad
		/// </summary>
		/// <param name="adUnitId">Ad id</param>
		public void OnUnityAdsShowStart(string adUnitId) { Advertisement.Banner.Hide(); }
		/// <summary>
		/// This method is called when we click on the ad
		/// </summary>
		/// <param name="adUnitId">Ad id</param>
		public void OnUnityAdsShowClick(string adUnitId) { }


		void OnDestroy()
		{
			// We clean up the button listeners
			showAdButton.onClick.RemoveAllListeners();
		}
	}
}
