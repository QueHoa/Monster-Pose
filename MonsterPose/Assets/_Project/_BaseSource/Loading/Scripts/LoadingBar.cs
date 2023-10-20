using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

     [SerializeField] private Image progressBar;
     [SerializeField] private TextMeshProUGUI progressText;

     public void Reset() {
          progressBar.fillAmount = 0f;
          progressText.text = "0%";
     }

     public void UpdateProgress(float progress, float duration) {
          progressBar.DOFillAmount(progress, duration).SetEase(Ease.InOutQuad);
          int currentPercent = progressBar.fillAmount.Percent();
          DOVirtual.Float(currentPercent, progress.Percent(), duration,
                          x => progressText.text = x.Int() + "%").SetEase(Ease.InOutQuad);
     }
}