using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace flow
{
    [RequireComponent(typeof(Ads.AdsInitializer))]
    [RequireComponent(typeof(Ads.BannerAd))]
    [RequireComponent(typeof(Ads.InterstitialAd))]
    public class AdsManager : MonoBehaviour
    {
        [SerializeField] 
        private GameManager.Scene scene;

        public static AdsManager Instance { get; private set; }

        private Ads.AdsInitializer initializer;
        private Ads.BannerAd bannerAd;

        private Ads.InterstitialAd interstitialAd;

        private InterstitialCallback interstitialCallback;
        private WaitInitialization waitInitialization;

        public delegate void InterstitialCallback();
        public delegate void WaitInitialization();

        public bool isInitialized { get; private set; }

        private void Awake()
        {
            if(Instance != null)
            {
                Instance.initOnScene(scene);
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            isInitialized = false;

            initializer = GetComponent<Ads.AdsInitializer>();
            bannerAd = GetComponent<Ads.BannerAd>();
            interstitialAd = GetComponent<Ads.InterstitialAd>();

            initializer.InitializeAds(onAdsInitialized);       
        }

        private void onAdsInitialized()
        {
            isInitialized = true;
            if (scene != GameManager.Scene.ChoosePack)
            {
                bannerAd.LoadBanner();
            }
            interstitialAd.LoadAd();

            if(waitInitialization != null)
                waitInitialization();
        }

        public void waitForInitialization(WaitInitialization waitInitialization)
        {
            this.waitInitialization = waitInitialization;
        }

        private void initOnScene(GameManager.Scene scene)
        {
            if (this.scene == GameManager.Scene.ChoosePack && scene != GameManager.Scene.ChoosePack)
            {
                bannerAd.LoadBanner();
            }
            else if (this.scene != GameManager.Scene.ChoosePack && scene == GameManager.Scene.ChoosePack)
            {
                bannerAd.hide();
            }

            this.scene = scene;
        }

        public void playInterstitialAd(InterstitialCallback callback)
        {
            this.interstitialCallback = callback;

            bannerAd.hide();
            interstitialAd.ShowAd(onInterstitialAdFinished);
            interstitialAd.LoadAd();
        }

        private void onInterstitialAdFinished(UnityAdsShowCompletionState completionState)
        {
            interstitialCallback();
            bannerAd.show();
        }

    }
}
