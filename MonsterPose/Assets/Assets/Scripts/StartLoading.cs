using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using DG.Tweening;
using OneHit;

public class StartLoading : MonoBehaviour
{
    [SerializeField]
    private GameObject uiMain;
    [SerializeField]
    private Image bar;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private CanvasGroup alpha;
    private bool isShowwingOpenAd = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Loading());
    }

    // Update is called once per frame
    void Update()
    {
        if (bar.fillAmount == 1 && !isShowwingOpenAd)
        {
            isShowwingOpenAd = true;

            MasterControl.Instance.ShowOpenAd((bool res) =>
            {
                DOTween.To(() => alpha.alpha, x => alpha.alpha = x, 0, 1f)
            .OnUpdate(() =>
            {
                if (alpha.alpha < 0.7f)
                {
                    uiMain.SetActive(true);
                }
                if (alpha.alpha < 0.1f)
                {
                    gameObject.SetActive(false);
                }
            });
            });
        }
    }
    IEnumerator Loading()
    {
        float t = 0f;
        while (t < 3.5f)
        {
            t += Time.deltaTime;
            bar.fillAmount = curve.Evaluate(t);
            yield return 0;
        }
    }
}
