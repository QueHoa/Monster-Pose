using System;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Purchasing;
using Cysharp.Threading.Tasks;

namespace OneHit
{

    public class MasterControl : MonoBehaviour
    {

        public static MasterControl Instance { get; private set; }

        [Header("Components")]
        public AdsManager _adsManager;
        public Purchaser _purchaser;
        public FirebaseManager _firebaseManager;

        private void Awake()
        {
            #region Singleton
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            #endregion
        }

        private async void Start()
        {
            _adsManager.Init();
            await UniTask.Delay(500);
            _purchaser.Init();
            await UniTask.Delay(500);
            _firebaseManager.Init();
        }

        #region =============== ADS ===============

        public void ShowBanner()
        {
            _adsManager.ShowBanner();
        }

        public void HideBanner()
        {
            _adsManager.HideBanner();
        }

        public void ShowInterAd(Action<bool> callback = null)
        {
            
            _adsManager.ShowInterstitial(callback);
        }

        public void ShowRewardAd(Action<bool> callback)
        {
            //AdLoadingPanel.Instance.TurnOn();
            //await UniTask.Delay(500);
            _adsManager.ShowRewardedVideo(callback);
        }

        public void ShowOpenAd(Action<bool> callback = null)
        {
            _adsManager.ShowAppOpenAd(callback);
        }

        #endregion



        #region  =============== IAP ===============

        [Header("IAP")]
        [InfoBox("<size=15>CHÚ Ý</size>: key đầu tiên luôn là <size=12>RemoveAds</size>")]
        public string[] productKeys;

        public void OnPurchased(string item)
        {
            Debug.Log("<color=lime> MasterControl: OnPurchased </color>" + item);

            if (item.Equals(productKeys[0]))
            {
                FirebaseManager.Instance.LogEvent(FirebaseEvent.PURCHASE_SUCCESS_NOADS);
                PrefInfo.SetAd(false);
                this.HideBanner();
            }
            //else if (item.Equals(productKeys[1])) {
            //    // TODO
            //}
            //else if (item.Equals(productKeys[2])) {
            //    // TODO
            //}
        }

        public void OnRestore(string item)
        {
            Debug.Log("<color=lime> MasterControl: OnRestore </color>" + item);

            if (item.Equals(productKeys[0]))
            {
                PrefInfo.SetAd(false);
                HideBanner();
            }
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            Debug.LogError("<color=red> FAILED TO PURCHASED </color> " + product.definition.id + " " + reason.ToString());

            if (reason.Equals(PurchaseFailureReason.DuplicateTransaction))
            {
                if (product != null)
                {
                    if (product.definition.id.Equals(productKeys[0]) && _purchaser.HasReceipt(product.definition.id))
                    {
                        OnPurchased(product.definition.id);
                    }
                }
            }
        }

        public void CheckRestore()
        {
            _purchaser.CheckRestore();
        }
        #endregion
    }
}