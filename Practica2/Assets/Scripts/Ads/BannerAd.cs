using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace flow.Ads
{
    public class BannerAd : MonoBehaviour
    {

        [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
        [SerializeField] string _adUnitId = "Banner_Android";

        void Start()
        {
            // Set the banner position:
            Advertisement.Banner.SetPosition(_bannerPosition);

            LoadBanner();
            ShowBannerAd();
        }

        // Implement a method to call when the Load Banner button is clicked:
        public void LoadBanner()
        {
            // Set up options to notify the SDK of load events:
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            // Load the Ad Unit with banner content:
            Advertisement.Banner.Load(_adUnitId, options);
        }

        // Implement code to execute when the loadCallback event triggers:
        void OnBannerLoaded()
        {
            Debug.Log("Banner loaded");
        }

        // Implement code to execute when the load errorCallback event triggers:
        void OnBannerError(string message)
        {
            Debug.Log($"Banner Error: {message}");
        }

        // Implement a method to call when the Show Banner button is clicked:
        void ShowBannerAd()
        {
            // Set up options to notify the SDK of show events:
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            // Show the loaded Banner Ad Unit:
            Advertisement.Banner.Show(_adUnitId, options);
        }

        void OnBannerClicked() { }
        void OnBannerShown() { }
        void OnBannerHidden() { }

    }

}