using System;
using UnityEngine;
using com.adjust.sdk;
using Firebase.Analytics;
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using OneHit.Framework;

namespace OneHit
{

    public enum AdsState
    {
        AdsProduction,
        UnlockAll
    }

    public class AdsManager : MonoBehaviour
    {

        [Space, GUIColor(0f, 0.8f, 1f)]
        public AdsState adsState;
        public GameObject fadeOpenAd;

        [Header("Time wait to reload if ad not ready")]
        public float interstitialReloadWaitTime = 0.8f;
        public float rewardedVideoReloadWaitTime = 1.2f;
        public float appOpenAdReloadWaitTime = 2f;

        [Header("Time between 2 ads in a row")]
        public static float timeAllowedToShowInterstitial = 15f;

        [Header("Check when ad loaded")]
        [ReadOnly] public bool readyShowBanner;
        [ReadOnly] public bool readyShowInterstitial;
        [ReadOnly] public bool readyShowRewardedVideo;
        [ReadOnly] public bool allowShowOpenAd = true;


        private IronSourceControl _ironSource;
        private AdmobControl _admob;

        private Action<bool> _interstitialCallback;
        private Action<bool> _rewardedVideoCallback;
        private Action<bool> _appOpenAdCallback;

        public bool AdsUnavailable => !PrefInfo.IsUsingAd()
                                   || !InternetConnection.HasInternet()
                                   || adsState == AdsState.UnlockAll;


        #region =============== INITIALIZATION ===============

        private void Awake()
        {
            _ironSource = GetComponentInChildren<IronSourceControl>();
            _admob = GetComponentInChildren<AdmobControl>();
        }

        public void Init(bool isConsent)
        {
            Debug.LogWarning("<color=orange> ADS MANAGER: Start init... </color>");
            _ironSource.Init(this, isConsent);
            _admob.Init(this);

            // set time to show ads
            SetTimeShowAds();
            Debug.Log($"<color=cyan> TimeToShowAds: {GetTimeShowAds()} </color>");
        }

        #endregion


        #region =============== BANNER VIEW ===============

        public void LoadBanner()
        {
            Debug.LogWarning("ADS MANAGER: Load Banner");

            if (AdsUnavailable)
            {
                Debug.LogError("ADS MANAGER: Ads Unavailable!");
                return;
            }

            _ironSource.LoadBanner();
        }

        public async void ShowBanner()
        {
            Debug.Log("ADS MANAGER: Show Banner");

            if (AdsUnavailable)
            {
                Debug.LogError("ADS MANAGER: Ads Unavailable!");
                return;
            }

            if (!readyShowBanner)
            {
                Debug.LogWarning("ADS MANAGER: Reload Banner");
                _ironSource.LoadBanner();

                Debug.LogWarning("ADS MANAGER: Wait until Banner ready");
                await new WaitUntil(() => readyShowBanner);
            }

            Debug.LogWarning("ADS MANAGER: Show Banner");
            _ironSource.ShowBanner();
        }

        public void HideBanner()
        {
            Debug.LogWarning("ADS MANAGER: Hide Banner");
            _ironSource.HideBanner();
        }

        public void OnBannerLoaded()
        {
            Debug.LogWarning("ADS MANAGER: On Banner Loaded");
            readyShowBanner = true;
        }

        public void OnBannerFailedToLoad()
        {
            Debug.LogError("ADS MANAGER: On Banner Failed To Load");
            readyShowBanner = false;
            LoadBanner();
        }

        #endregion


        #region =============== INTERSTITIAL ===============

        public void LoadInterstitial()
        {
            if (AdsUnavailable)
            {
                Debug.LogError("ADS MANAGER: AdsUnavailable!");
                return;
            }

            Debug.LogWarning("ADS MANAGER: Load Interstitial");
            _ironSource.LoadInterstitial();
        }

        public async void ShowInterstitial(Action<bool> callback)
        {
            _interstitialCallback = callback;

#if UNITY_EDITOR
            Debug.Log("<color=lime> ADS MANAGER: Skip Interstitial in UNITY_EDITOR </color>");
            this.OnInterstitialCallback(true);
            AdLoadingPanel.Instance.TurnOff();
            return;
#endif

            if (adsState == AdsState.UnlockAll || !PrefInfo.IsUsingAd())
            {
                Debug.LogWarning("ADS MANAGER: Skip Interstitial (unlock all or not using ad)");
                this.OnInterstitialCallback(true);
                AdLoadingPanel.Instance.TurnOff();
                return;
            }
            if (!InternetConnection.HasInternet() || !IsAllowShowInterstitial())
            {
                Debug.LogWarning("ADS MANAGER: Skip Interstitial (no internet or not allow show)");
                this.OnInterstitialCallback(false);
                AdLoadingPanel.Instance.TurnOff();
                return;
            }

            if (!readyShowInterstitial)
            {
                Debug.LogWarning("ADS MANAGER: Reload Interstitial");
                _ironSource.LoadInterstitial();

                Debug.LogWarning($"ADS MANAGER: Wait until Interstitial ready (no more {interstitialReloadWaitTime}s)");
                float waitTimeLoadAd = 0f;
                while (!readyShowInterstitial && waitTimeLoadAd < interstitialReloadWaitTime)
                {
                    waitTimeLoadAd += 0.1f;
                    await UniTask.Delay(100);
                }
            }

            if (readyShowInterstitial)
            {
                AdLoadingPanel.Instance.TurnOn();
                await UniTask.Delay(1000);
                Debug.LogWarning("ADS MANAGER: Ready show Interstitial");
                _ironSource.ShowInterstitial();
            }
            else
            {
                Debug.LogError("ADS MANAGER: Not ready show Interstitial");
                this.OnInterstitialCallback(false);
            }
            AdLoadingPanel.Instance.TurnOff();
        }

        public void OnInterstitialReady()
        {
            Debug.LogWarning("ADS MANAGER: On Interstitial Ready");
            readyShowInterstitial = true;
        }

        public void OnInterstitialLoadFailed()
        {
            Debug.LogWarning("ADS MANAGER: On Interstitial Load Failed");
            readyShowInterstitial = false;
            //this.LoadInterstitial();
        }

        public void OnInterstitialOpen()
        {
            Debug.LogWarning("ADS MANAGER: On Interstitial Open");
            AudioManager.ChangePitch(0);
            Time.timeScale = 0;

            allowShowOpenAd = false;
            readyShowInterstitial = false;
        }

        public void OnInterstitalClose()
        {
            Debug.LogWarning("ADS MANAGER: On Interstital Close");
            AudioManager.ChangePitch(1);
            Time.timeScale = 1;

            allowShowOpenAd = true;

            PrefInfo.SetTimeShowAds();
            this.LoadInterstitial();
        }

        public void OnInterstitialShowSucceeded()
        {
            Debug.LogWarning("ADS MANAGER: On Interstitial Show Succeeded");
            FirebaseManager.Instance.LogEvent(FirebaseEvent.ADS_INTERSTITIAL);

            this.OnInterstitialCallback(true);
            //this.LoadInterstitial();
        }

        public void OnInterstitialShowFailed()
        {
            Debug.LogWarning("ADS MANAGER: On Interstitial Show Failed");
            this.OnInterstitialCallback(false);
            //this.LoadInterstitial();
        }

        public void OnInterstitialCallback(bool showSuccess)
        {
            Debug.LogWarning($"ADS MANAGER: On Interstitial Callback ({showSuccess})");
            _interstitialCallback?.Invoke(showSuccess);
            _interstitialCallback = null;
        }

        #endregion


        #region =============== REWARDED VIDEO ===============

        public void LoadRewardedVideo()
        {
            if (AdsUnavailable) return;

            Debug.LogWarning("ADS MANAGER: Load Rewarded Video");
            _ironSource.LoadRewardedVideo();
        }

        public async void ShowRewardedVideo(Action<bool> callback)
        {
            _rewardedVideoCallback = callback;

            if (InternetConnection.Instance.isTestNoInternet)
            {
                Debug.LogError("ADS MANAGER: is test no internet");
                this.OnRewardedVideoCallback(false);
                InternetConnection.Instance.ShowVideoNotReadyPanel();
                //AdLoadingPanel.Instance.TurnOff();
                return;
            }

#if UNITY_EDITOR
            Debug.Log("<color=lime> ADS MANAGER: Skip Rewarded Video in UNITY_EDITOR </color>");
            this.OnRewardedVideoCallback(true);
            AdLoadingPanel.Instance.TurnOff();
            return;
#endif

            if (adsState == AdsState.UnlockAll)
            {
                Debug.Log("ADS MANAGER: Skip Rewarded Video (unlock all or not using ad)");
                this.OnRewardedVideoCallback(true);
                AdLoadingPanel.Instance.TurnOff();
                return;
            }
            if (!InternetConnection.HasInternet())
            {
                Debug.LogError("ADS MANAGER: Skip Rewarded Video (no internet)");
                this.OnRewardedVideoCallback(false);
                AdLoadingPanel.Instance.TurnOff();
                return;
            }

            if (!readyShowRewardedVideo)
            {
                Debug.LogWarning("ADS MANAGER: Reload Rewarded Video");
                _ironSource.LoadRewardedVideo();

                Debug.LogWarning($"ADS MANAGER: Wait until Rewarded Video ready (no more {rewardedVideoReloadWaitTime}s)");
                float waitTimeLoadAd = 0f;
                while (!readyShowRewardedVideo && waitTimeLoadAd < rewardedVideoReloadWaitTime)
                {
                    waitTimeLoadAd += 0.1f;
                    await UniTask.Delay(100);
                }
            }

            if (readyShowRewardedVideo)
            {
                AdLoadingPanel.Instance.TurnOn();
                await UniTask.Delay(1000);
                Debug.LogWarning("ADS MANAGER: Ready show Rewarded Video");
                _ironSource.ShowRewardedVideo();
            }
            else
            {
                Debug.LogError("ADS MANAGER: Not ready show Rewarded Video");
                this.OnRewardedVideoCallback(false);
            }
            AdLoadingPanel.Instance.TurnOff();

        }

        public void OnRewardedVideoAvailable()
        {
            Debug.LogWarning("ADS MANAGER: On Rewarded Video Available");
            readyShowRewardedVideo = true;
        }

        public void OnRewardedVideoUnavailable()
        {
            Debug.LogWarning("ADS MANAGER: On Rewarded Video Unavailable");
            readyShowRewardedVideo = false;
            //this.LoadRewardedVideo();
        }

        public void OnRewardedVideoOpen()
        {
            Debug.LogWarning("ADS MANAGER: On Rewarded Video Open");
            AudioManager.ChangePitch(0);
            Time.timeScale = 0;

            allowShowOpenAd = false;
            readyShowRewardedVideo = false;
        }

        public void OnRewardedVideoClose()
        {
            Debug.LogWarning("ADS MANAGER: On Rewarded Video Close");
            AudioManager.ChangePitch(1);
            Time.timeScale = 1;

            allowShowOpenAd = true;

            PrefInfo.SetTimeShowAds();
            this.LoadRewardedVideo();
        }

        public void OnRewardedVideoShowSucceeded()
        {
            Debug.LogWarning("ADS MANAGER: On Rewarded Video Show Succeeded");
            FirebaseManager.Instance.LogEvent(FirebaseEvent.ADS_REWARD);

            this.OnRewardedVideoCallback(true);
            //this.LoadRewardedVideo();
        }

        public void OnRewardedVideoShowFailed()
        {
            Debug.LogWarning("ADS MANAGER: On Rewarded Video Show Failed");
            this.OnRewardedVideoCallback(false);
            //this.LoadRewardedVideo();
        }

        public void OnRewardedVideoCallback(bool showSuccess)
        {
            Debug.LogWarning($"ADS MANAGER: On Rewarded Callback ({showSuccess})");
            _rewardedVideoCallback?.Invoke(showSuccess);
            _rewardedVideoCallback = null;
        }
        #endregion


        #region =============== APP OPEN AD ===============

        public void LoadAppOpenAd()
        {
            Debug.LogWarning("ADS MANAGER: Load app open ad");

            if (AdsUnavailable)
            {
                Debug.LogError("ADS MANAGER: Ads unavailable");
                return;
            }
            _admob.LoadAppOpenAd();
        }

        public async void ShowAppOpenAd(Action<bool> callback)
        {
            _appOpenAdCallback = callback;

            //#if UNITY_EDITOR
            //               Debug.Log("<color=lime> ADS MANAGER: Skip app open ad in UNITY_EDITOR </color>");
            //               OnAppOpenAdCallback(true);
            //               return;
            //#endif
            if (adsState == AdsState.UnlockAll || !PrefInfo.IsUsingAd())
            {
                Debug.LogWarning("ADS MANAGER: Skip app open ad (Unlock all || Ads removed)");
                OnAppOpenAdCallback(true);
                return;
            }
            if (!allowShowOpenAd || !InternetConnection.HasInternet())
            {
                Debug.LogWarning("ADS MANAGER: Skip app open ad (Not allow show || No internet)");
                OnAppOpenAdCallback(false);
                return;
            }

            if (!_admob.IsOpenAdAvailable)
            {
                Debug.LogWarning("ADS MANAGER: Reload app open ad");
                _admob.LoadAppOpenAd();

                Debug.LogWarning($"ADS MANAGER: Wait until app open ad ready (no more {appOpenAdReloadWaitTime}s)");
                var timeOut = DateTime.Now.AddSeconds(appOpenAdReloadWaitTime);
                await new WaitUntil(() => _admob.IsOpenAdAvailable || DateTime.Now > timeOut);
            }

            if (_admob.IsOpenAdAvailable)
            {
                Debug.LogWarning("ADS MANAGER: Ready show app open ad");
                fadeOpenAd.SetActive(true);
                await UniTask.Delay(50);
                _admob.ShowAppOpenAd();
            }
            else
            {
                Debug.LogError("ADS MANAGER: Not ready show app open ad");
                OnAppOpenAdCallback(false);
            }
        }

        public void OnAppOpenAdCallback(bool showSuccess)
        {
            Debug.LogWarning($"ADS MANAGER: On app open ad callback: ({showSuccess})");
            fadeOpenAd.SetActive(false);
            _appOpenAdCallback?.Invoke(showSuccess);
            _appOpenAdCallback = null;
        }

        public void OnAppOpenAdOpen()
        {
            Debug.LogWarning("ADS MANAGER: On App Open Ad Open");
            allowShowOpenAd = false;
        }

        public void OnAppOpenAdClose()
        {
            Debug.LogWarning("ADS MANAGER: On App Open Ad Close");
            FirebaseManager.Instance.LogEvent(FirebaseEvent.OPEN_AD);

            allowShowOpenAd = true;

            OnAppOpenAdCallback(true);
            LoadAppOpenAd();
        }

        public void OnAppOpenAdFailed()
        {
            Debug.LogWarning("ADS MANAGER: On App Open Ad Failed");
            PrefInfo.SetTimeShowAds();
            allowShowOpenAd = true;

            OnAppOpenAdCallback(false);
            //LoadAppOpenAd();
        }

        public void OnAppOpenAdPaid(AdValue adValue)
        {
            Debug.LogWarning("ADS MANAGER: On App Open Ad Paid");
            double revenue = adValue.Value / 1000000f;
            var imp = new[] {
                    new Parameter("ad_platform", "admob"),
                    new Parameter("ad_source", "admob"),
                    new Parameter("ad_unit_name", "open_ads"),
                    new Parameter("ad_format", "open_ads"),
                    new Parameter("value", revenue),
                    new Parameter("currency", adValue.CurrencyCode)
               };
            FirebaseAnalytics.LogEvent("ad_impression", imp);

            AdjustAdRevenue adjustEvent = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
            //most important is calling setRevenue with two parameters
            adjustEvent.setRevenue(revenue, adValue.CurrencyCode);
            //Sent event to Adjust server
            Adjust.trackAdRevenue(adjustEvent);
        }
        public void OnAppOpenAdSDKPaid(AdValue adValue)
        {
            Debug.LogWarning("ADS MANAGER: On App Open Ad Paid");
            double revenue = adValue.Value / 1000000f;
            var imp = new[] {
                    new Parameter("ad_platform", "admob"),
                    new Parameter("ad_source", "admob"),
                    new Parameter("ad_unit_name", "open_ads"),
                    new Parameter("ad_format", "open_ads"),
                    new Parameter("value", revenue),
                    new Parameter("currency", adValue.CurrencyCode)
               };
            FirebaseAnalytics.LogEvent("cc_openad_native_revenue", imp);
        }
        #endregion


        public bool IsAllowShowInterstitial()
        {
            DateTime lastTimeShowAd = DateTime.Parse(GetTimeShowAds());
            double time = (DateTime.Now - lastTimeShowAd).TotalSeconds;
            return time >= timeAllowedToShowInterstitial; //! không show 2 inter ads liên tiếp trong 1 khoảng thời gian
        }

        //-------------------------------------------------------------------------------------

        public static string GetTimeShowAds()
        {
            return PlayerPrefs.GetString("TimeToShowAds", new DateTime(1990, 1, 1).ToString());
        }

        public static void SetTimeShowAds()
        {
            PlayerPrefs.SetString("TimeToShowAds", DateTime.Now.ToString());
        }
    }
}