using System;
using UnityEngine;
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;
using GoogleMobileAds.Common;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace OneHit
{

    public class AdmobControl : MonoBehaviour
    {

        [Header("Key")]
        [SerializeField, GUIColor(0f, 0.8f, 1f)] private string androidAppOpenAdID;
        [SerializeField, GUIColor(0f, 0.8f, 1f)] private string iosAppOpenAdID;
        public GameObject fade;

        [Header("Test")]
        [SerializeField] private bool testAppOpenAd;
        [ShowInInspector, ReadOnly] private string androidAppOpenAdTestID = "ca-app-pub-3940256099942544/3419835294";
        [ShowInInspector, ReadOnly] private string iosAppOpenAdTestID = "ca-app-pub-3940256099942544/5662855259";

        private AdsManager _adsManager;
        private AppOpenAd _appOpenAd;
        private DateTime _adExpireTime; // thời gian hết hạn của app open ad
        private string _adUnitId;

        public bool IsOpenAdAvailable => _appOpenAd != null
                                      && _appOpenAd.CanShowAd()
                                      && DateTime.Now < _adExpireTime;


        #region =============== HANDLE ===============

        public void Init(AdsManager adsManager)
        {
            Debug.LogWarning("Admob: Start init...");
            _adsManager = adsManager;

#if UNITY_ANDROID
            _adUnitId = testAppOpenAd || string.IsNullOrEmpty(androidAppOpenAdID)
                      ? androidAppOpenAdTestID
                      : androidAppOpenAdID;
#elif UNITY_IPHONE
               _adUnitId = testAppOpenAd || string.IsNullOrEmpty(iosAppOpenAdID)
                         ? iosAppOpenAdTestID
                         : iosAppOpenAdID;
#endif

            MobileAds.Initialize((initStatus) =>
            {
                Debug.LogWarning("Admob: Init complete!");
                _adsManager.LoadAppOpenAd();
            });
        }

        public void LoadAppOpenAd()
        {
            // Clean up the old ad before loading a new one.
            if (_appOpenAd != null)
            {
                _appOpenAd.Destroy();
                _appOpenAd = null;
            }

            Debug.LogWarning("Admob: Loading the app open ad...");
            AppOpenAd.Load(_adUnitId, ScreenOrientation.Portrait, new AdRequest.Builder().Build(), (ad, error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Admob: App open ad failed to load an ad with error: " + error);
                    return;
                }

                // App open ad is loaded.
                Debug.LogWarning("Admob: App open ad loaded with response: " + ad.GetResponseInfo());
                _adExpireTime = DateTime.Now + TimeSpan.FromHours(4); // App open ads can be preloaded for up to 4 hours.
                _appOpenAd = ad;

                RegisterAppOpenAdEvents();
            });
        }

        public void ShowAppOpenAd()
        {
            Debug.LogWarning("Admob: Show app open ad!");
            _appOpenAd.Show();
        }

        #endregion


        #region  =============== APP OPEN AD EVENTS ===============

        private void RegisterAppOpenAdEvents()
        {
            _appOpenAd.OnAdFullScreenContentOpened += OnAdFullScreenContentOpened;
            _appOpenAd.OnAdFullScreenContentClosed += OnAdFullScreenContentClosed;
            _appOpenAd.OnAdFullScreenContentFailed += OnAdFullScreenContentFailed;
            _appOpenAd.OnAdImpressionRecorded += OnAdImpressionRecorded;
            _appOpenAd.OnAdClicked += OnAdClicked;
            _appOpenAd.OnAdPaid += OnAdPaid;
        }

        // Raised when an ad opened full screen content
        private void OnAdFullScreenContentOpened()
        {
            Debug.Log("Admob: App open ad full screen content opened");
            _adsManager.OnAppOpenAdOpen();
        }

        // Raised when the ad closed full screen content
        private void OnAdFullScreenContentClosed()
        {
            Debug.Log("Admob: App open ad full screen content closed.");
            _adsManager.OnAppOpenAdClose();
        }

        // Raised when the ad failed to open full screen content.
        private void OnAdFullScreenContentFailed(AdError error)
        {
            Debug.LogError("Admob: App open ad failed to open full screen content with error " + error.GetMessage());
            _adsManager.OnAppOpenAdFailed();
        }

        // Raised when a click is recorded for an ad
        private void OnAdClicked()
        {
            Debug.Log("Admob: App open ad was clicked");
        }

        // Raised when an impression is recorded for an ad
        private void OnAdImpressionRecorded()
        {
            Debug.Log("Admob: App open ad recorded an impression");
        }

        // Raised when the ad is estimated to have earned money
        private void OnAdPaid(AdValue adValue)
        {
            // khong xu ly

            if (adValue != null)
            {
                Debug.Log($"Admob: App open ad paid value: {adValue.Value}, {adValue.CurrencyCode})");
                _hasAdImpression = true;
                _adValue = adValue;
            }
            else
            {
                Debug.LogError("ADS MANAGER: open ad impression data is null!");
            }
        }
        #endregion


        #region  =============== APP STATE CHANGED ===============

        private void Awake()
        {
            // Use the AppStateEventNotifier to listen to application open/close events.
            AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
        }

        private void OnDestroy()
        {
            // Always unlisten to events when complete.
            AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
        }

        private void OnAppStateChanged(AppState state)
        {
            Debug.LogWarning("Admob: App State changed to " + state);

            // if the app is Foregrounded and the ad is available, show it.
            if (state == AppState.Foreground)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    return;

                _adsManager.ShowAppOpenAd(res =>
                    {

                    });
            }
        }

        #endregion


        private bool _hasAdImpression;
        private AdValue _adValue;

        private void Update()
        {
            if (_hasAdImpression)
            {
                _adsManager.OnAppOpenAdPaid(_adValue);
                _adsManager.OnAppOpenAdSDKPaid(_adValue);
                _hasAdImpression = false;
                _adValue = null;
            }
        }
    }
}