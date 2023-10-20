using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace BaseSource.Settings
{
     public class DarkBgPanel : MonoBehaviour
     {
          [Space]
          [SerializeField] protected Image darkBG;
          [SerializeField] protected Button close;
          [SerializeField] protected Popup popup;

          [Space]
          [SerializeField] protected float activeDuration = 0.4f;
          [SerializeField] protected float inactiveDuration = 0.3f;

          protected virtual void Awake()
          {
               GetComponent<CanvasGroup>().alpha = 1f;
          }

          protected void OnValidate()
          {
               darkBG = GetComponent<Image>();
               popup = GetComponentInChildren<Popup>();
          }

          [Button("Show")]
          public void Show()
          {
               Debug.Log("Show");
               float alpha = GetComponent<CanvasGroup>().alpha;
               GetComponent<CanvasGroup>().alpha = 1f - alpha;
          }

          protected virtual void Start()
          {
               gameObject.SetActive(false);
               close.interactable = false;
               darkBG.Fade(0f);
               popup.Hide();
          }

          public virtual void Enable()
          {
               gameObject.SetActive(true);
               close.interactable = true;

               darkBG.DOKill();
               darkBG.DOFade(1f, activeDuration)
                     .SetEase(Ease.OutCubic);

               popup.Appear();
          }

          public virtual void Disable()
          {
               close.interactable = false;

               darkBG.DOKill();
               darkBG.DOFade(0f, inactiveDuration)
                     .SetEase(Ease.OutCubic)
                     .OnComplete(() =>
                     {
                          gameObject.SetActive(false);
                     });

               popup.Disappear();
          }
     }
}