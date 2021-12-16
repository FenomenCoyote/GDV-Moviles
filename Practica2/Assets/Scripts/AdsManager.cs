using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace flow
{
    [RequireComponent(typeof(Ads.AdsInitializer))]
    [RequireComponent(typeof(Ads.BannerAd))]
    [RequireComponent(typeof(Ads.InterstitialAd))]

    public class AdsManager : MonoBehaviour
    {
        [SerializeField] GameManager.Scene scene;

        public static AdsManager Instance { get; private set; }

        private Ads.AdsInitializer initializer;
        private Ads.BannerAd bannerAd;
        private Ads.InterstitialAd interstitialAd;

        private void Awake()
        {
            if(Instance != null)
            {
                initOnScene(scene);
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if(scene != GameManager.Scene.ChoosePack)
            {

            }
        }

        private void initOnScene(GameManager.Scene scene)
        {

        }
    }

}
