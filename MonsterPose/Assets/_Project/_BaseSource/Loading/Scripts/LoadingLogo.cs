using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingLogo : MonoBehaviour {

     private Image image;
     private RectTransform rectTransform;
     private float targetAnchorPosY;

     private void Awake() {
          image = GetComponent<Image>();
          rectTransform = GetComponent<RectTransform>();
          targetAnchorPosY = rectTransform.anchoredPosition.y;
     }

     public void Reset() {
          image.Fade(0f);
          rectTransform.anchoredPosition = Vector3.zero;
     }

     public void Show() {
          image.DOFade(1f, 0.5f);
          rectTransform.DOAnchorPosY(targetAnchorPosY, 0.5f).SetEase(Ease.OutBack);
     }
}