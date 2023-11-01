using System;
using Firebase;
using UnityEngine;
using System.Collections;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using Sirenix.OdinInspector;

namespace OneHit
{

    public class FirebaseManager : MonoBehaviour
    {

        public static FirebaseManager Instance { get; private set; }

        [ShowInInspector, ReadOnly] private bool isInitialized;
        [ShowInInspector, ReadOnly] private bool fetchDone;
        private LastFetchStatus result;

        DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            FetchData();
        }

        public void Init()
        {
            Debug.Log("<color=orange> Firebase: init </color>");

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                Debug.Log("<color=lime> Firebase: initialized </color>");
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                    InitializeFirebase();
                else
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            });
        }

        private void InitializeFirebase()
        {
            Debug.Log("<color=cyan> Firebase: Enabling data collection </color>");
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            isInitialized = true;
        }

        private void FetchData()
        {
            StartCoroutine(WaitForFetch());
            StartCoroutine(WaitForFetchDone());
        }

        private IEnumerator WaitForFetch()
        {
            yield return new WaitUntil(() => isInitialized);
            Fetch();
        }

        private IEnumerator WaitForFetchDone()
        {
            yield return new WaitUntil(() => fetchDone);

            if (result.Equals(LastFetchStatus.Success))
            {
                try
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync();

                    ConfigValue ads_time = FirebaseRemoteConfig.DefaultInstance.GetValue("TimeBetweenEachAds");
                    AdsManager.timeAllowedToShowInterstitial = ads_time.LongValue;
                    Debug.Log("Time between each ads:" + ads_time.LongValue);
                    ConfigValue allowAds = FirebaseRemoteConfig.DefaultInstance.GetValue("AdEnable");
                    PrefInfo.adEnable = allowAds.BooleanValue;
                    ConfigValue levelAd = FirebaseRemoteConfig.DefaultInstance.GetValue("LevelStartShowAds");
                    GameManager.levelShowAd = levelAd.LongValue;

                    //if (MasterControl.Instance != null)
                    //{
                    //    MasterControl.Instance.adsController.LoadRule();
                    //}
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
            else if (result.Equals(LastFetchStatus.Failure))
            {
                Debug.LogError("<color=red> Firebase: Faile to load </color>");
            }
            else
            {
                Debug.LogWarning("<color=orange> Firebase: pending </color>");
            }
        }

        private void Fetch()
        {
            Debug.Log("<color=cyan> Fetching data... </color>");
            try
            {
                FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWith(task2 =>
                {
                    ConfigInfo info = FirebaseRemoteConfig.DefaultInstance.Info;
                    result = info.LastFetchStatus;
                    if (info.LastFetchStatus.Equals(LastFetchStatus.Success))
                    {
                        fetchDone = true;
                    }
                    else if (info.LastFetchStatus.Equals(LastFetchStatus.Failure))
                    {
                        Debug.LogError("Firebase: fail to load");
                    }
                    else
                    {
                        Debug.LogWarning("Firebase: pending");
                    }
                });
            }
            catch (Exception e)
            {
                Debug.LogError("<color=red> LOI 5108410284: " + e.ToString() + "</color>");
            }
        }

        public void LogEvent(string eventName)
        {
            StartCoroutine(DoLogEvent(eventName));
        }

        private IEnumerator DoLogEvent(string eventName)
        {
            yield return new WaitForSeconds(1);
            float timer = 4f;
            while (!isInitialized && timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0) break;
                yield return null;
            }
            try
            {
                Debug.Log($"<color=orange> Firebase Log: </color> <color=cyan> {eventName} </color>");
                FirebaseAnalytics.LogEvent(eventName);
            }
            catch (FirebaseException e)
            {
                Debug.LogError(e);
            }
        }
    }
}