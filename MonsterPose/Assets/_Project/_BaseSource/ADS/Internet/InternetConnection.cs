using System;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using BaseSource.Settings;

namespace OneHit {

     public class InternetConnection : MonoBehaviour {

          public static InternetConnection Instance { get; private set; }

          [Header("Components")]
          [SerializeField] private DarkBgPanel noInternetPanel;
          [SerializeField] private DarkBgPanel videoNotReadyPanel;


          [Header("Config")]
          [InfoBox("Yêu cầu Internet để chơi game?\n" + "Trên Editor thì tắt Wifi đi để test nhé!", InfoMessageType.Warning)]
          [SerializeField] private bool requireInternet;

          [ShowIf("requireInternet", true)]
          [SerializeField] private float timePerCheckInternet = 1f;

          [ShowIf("requireInternet", true)]
          [ShowInInspector, ReadOnly] private bool canCheckInternet;


          [Header("Test")]
          [InfoBox("True: luôn luôn NoInternetPanel khi hiện RewardedVideo\n" +
                   "False: luôn luôn bỏ qua RewardedVideo trên editor")]
          public bool isTestNoInternet;


          private void Awake() => Instance = this;

          private void Start() {
               LogInternetStatus();
               StartCoroutine(WaitCheckInternet());
          }

          private IEnumerator WaitCheckInternet() {
               // not check internet if not require
               if (!requireInternet) yield break;

               // wait loading scene complete
               yield return new WaitUntil(() => canCheckInternet);

               Debug.LogWarning("<color=cyan> Internet: Start check internet </color>");
               var waitForSeconds = new WaitForSeconds(timePerCheckInternet);

               while (true) {
                    yield return waitForSeconds;

                    if (!HasInternet()) {
                         noInternetPanel.Enable();
                         yield return new WaitUntil(HasInternet);
                         noInternetPanel.Disable();
                    }
               }
          }

          private void LogInternetStatus() {
               if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                    Debug.LogWarning("<color=lime> Internet: Network is available through wifi! </color>");

               if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
                    Debug.LogWarning("<color=lime> Internet: Network is available through mobile data! </color>");

               if (Application.internetReachability == NetworkReachability.NotReachable)
                    Debug.LogError("<color=red> Internet: Network not available! </color>");
          }

          public void CheckInternetAfterLoading() {
               canCheckInternet = true;
          }

          public void OpenWifiSetting() {
#if UNITY_ANDROID
               try {
                    AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent");
                    intent.Call<AndroidJavaObject>("setAction", "android.settings.WIFI_SETTINGS");

                    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
                    currentActivity.Call("startActivity", intent);
               }
               catch (Exception e) {
                    Debug.LogError(e);
               }
#endif
          }

          public void ShowVideoNotReadyPanel() {
               videoNotReadyPanel.Enable();
          }

          public static bool HasInternet() {
               return Application.internetReachability != NetworkReachability.NotReachable;
          }
     }
}