using UnityEngine;

public class LargeNativeAds : NativeAdComponent {

     private int playCount;

     private void OnEnable() {
          TryShow();
     }

     public override void TryShow() {
          if (!PrefInfo.IsUsingAd()) return;
          if (adReadyToShow) {
               gameObject.SetActive(true);
          }
          RequestNativeAdHandle();

          playCount = 0;
     }

     private void OnPlay() {
          playCount++;
          if (playCount % 2 == 0)
               RequestNativeAdHandle();
     }

     public override void AdLoadedHandle() {
          Debug.LogWarning("Native ad: Large ad loaded");
          gameObject.SetActive(true);
     }

     public override void AdFailedLoadHandle() {
          Debug.LogError("Native ad: Large ad load failed");
     }
}