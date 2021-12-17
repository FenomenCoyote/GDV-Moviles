using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

namespace flow.Ads
{
    public class BannerAd : MonoBehaviour
    {

        [SerializeField] BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;
        [SerializeField] string adUnitId = "Banner_Android";

        void Start()
        {
            //We set the banner position
            Advertisement.Banner.SetPosition(bannerPosition);
        }

        /// <summary>
        /// Loads the banner
        /// </summary>
        public void LoadBanner()
        {
            // We set up options to notify the SDK of load events
            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            //We load the Ad Unit with banner content
            Advertisement.Banner.Load(adUnitId, options);
        }

        /// <summary>
        /// This method is called when the loadCallback event triggers,
        /// it shows the banner when is loaded
        /// </summary>
        void OnBannerLoaded()
        {
            Debug.Log("Banner loaded");
            ShowBannerAd();
        }

        /// <summary>
        /// This method is called when the errorCallback event triggers
        /// </summary>
        void OnBannerError(string message)
        {
            Debug.Log($"Banner Error: {message}");
        }

        /// <summary>
        /// Shows the banner
        /// </summary>
        void ShowBannerAd()
        {
            //We set up options to notify the SDK of show events
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            //We show the loaded banner ad
            Advertisement.Banner.Show(adUnitId, options);
        }

        /// <summary>
        /// Hides the banner
        /// </summary>
        public void hide()
        {
            Advertisement.Banner.Hide();
        }

        /// <summary>
        /// Shows the banner
        /// </summary>
        public void show()
        {
            Advertisement.Banner.Show(adUnitId);
        }

        /// <summary>
        /// This method is called when the clickCallback event triggers
        /// </summary>
        void OnBannerClicked() { }
        /// <summary>
        /// This method is called when the showCallback event triggers
        /// </summary>
        void OnBannerShown() { }
        /// <summary>
        /// This method is called when the hideCallback event triggers
        /// </summary>
        void OnBannerHidden() { }
    }
}