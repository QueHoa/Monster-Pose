using UnityEngine;
using System.Threading.Tasks;

namespace OneHit {

     public class RemoveAdsButton : MonoBehaviour {

          /// <summary>
          /// set active/inactive when start scene
          /// </summary>
          private void OnEnable() {
               gameObject.SetActive(PrefInfo.IsUsingAd());
          }

          /// <summary>
          /// set active false for IAP Button 0.1s after purchased
          /// not set active immediately because IAP Button need a bit time to handle purchase
          /// </summary>
          public async void HideAfterPurchased() {
               await Task.Delay(100);
               gameObject.SetActive(false);
          }
     }
}