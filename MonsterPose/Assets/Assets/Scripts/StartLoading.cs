using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class StartLoading : MonoBehaviour
{
    [SerializeField]
    private GameObject uiMain;
    [SerializeField]
    private GameObject BGSplash;
    [SerializeField]
    private Animator animSplash;
    [SerializeField]
    private Image bar;
    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private CanvasGroup alpha;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Splash());
    }

    // Update is called once per frame
    void Update()
    {
        if (bar.fillAmount == 1)
        {
            alpha.alpha -= Time.deltaTime;
            if(alpha.alpha < 0.7f)
            {
                uiMain.SetActive(true);
                if(alpha.alpha < 0.1f)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
    IEnumerator Splash()
    {
        yield return new WaitForSeconds(1.5f);
        animSplash.SetTrigger("hide-splash");
        yield return new WaitForSeconds(1.5f);
        BGSplash.SetActive(false);
        float t = 0f;
        while (t < 3.5f)
        {
            t += Time.deltaTime;
            bar.fillAmount = curve.Evaluate(t);
            yield return 0;
        }        
    }
}
