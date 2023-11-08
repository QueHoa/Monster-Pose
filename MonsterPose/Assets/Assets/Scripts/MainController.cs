using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;
using OneHit;

public class MainController : MonoBehaviour
{
    [HideInInspector]
    public int numberPlaying;
    [HideInInspector]
    public bool isHint;
    [HideInInspector]
    public bool isWin;

    [SerializeField]
    private Image bg;
    [SerializeField]
    private Sprite[] background;
    [SerializeField]
    private Text textLevel;
    public Transform clock;
    [SerializeField]
    private Image sliceTime;
    [SerializeField]
    private Text txtTime;
    [SerializeField]
    private GameObject home;
    [SerializeField]
    private Animator losePanel;
    [SerializeField]
    private GameObject endGame;
    [SerializeField]
    private GameObject trailerMode;
    public GameObject panelSetting;
    public GameObject loading;
    public RectTransform buttonBack;
    public RectTransform buttonSuggest;
    public RectTransform buttonSkip;
    public RectTransform buttonTextLevel;
    public RectTransform buttonTime;
    private float time;
    private int timeInSecond;
    private int unlockedLevelsNumber;
    private int unlockedModeNumber;
    private int numberTictac;
    private bool startTime;

    // Start is called before the first frame update
    private void Awake()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        numberPlaying = unlockedLevelsNumber - 1;
    }

    private void Start()
    {
        InternetConnection.Instance.CheckInternetAfterLoading();
        AudioManager.Play("new_level");
    }

    private void OnEnable()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_PLAY_" + (numberPlaying + 1));
        buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
        buttonSuggest.anchoredPosition = new Vector3(260.2f, buttonSuggest.anchoredPosition.y, 0);
        buttonSkip.anchoredPosition = new Vector3(113.8f, buttonSkip.anchoredPosition.y, 0);
        buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
        buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
        losePanel.gameObject.SetActive(false);
        if (numberPlaying + 1 <= 20)
        {
            time = 20.5f;
        }
        else if (numberPlaying + 1 > 20 && numberPlaying + 1 <= 50)
        {
            time = 25.5f;
        }
        else
        {
            time = 30.5f;
        }
        numberTictac = 0;
        sliceTime.fillAmount = 1;
        ChangeBG();
        
        isHint = false;
        isWin = false;
        buttonBack.DOAnchorPosX(102, 0.8f).SetEase(Ease.OutQuart);
        buttonSuggest.DOAnchorPosX(-260.2f, 0.8f).SetEase(Ease.OutQuart);
        buttonSkip.DOAnchorPosX(-113.8f, 0.8f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(-140, 0.8f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(-283, 0.8f).SetEase(Ease.OutQuart);
        startTime = false;
        AudioManager.Play("new_level");
    }
    // Update is called once per frame
    void Update()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        if (!losePanel.gameObject.activeInHierarchy && !endGame.activeInHierarchy)
        {
            if (numberPlaying != 0 && !startTime)
            {
                startTime = true;
            }
            if(numberPlaying == 0 && !startTime)
            {
                StartCoroutine(StartTime());
            }
            if (startTime)
            {
                time -= Time.deltaTime;
            }
            if (time <= 0)
            {
                time = 0;
            }
            if (timeInSecond == 5 && numberTictac == 0)
            {
                AudioManager.Play("tictac");
                clock.DORotate(new Vector3(0, 0, -6), 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            clock.DORotate(Vector3.zero, 0.05f).SetEase(Ease.OutSine);
                        });
                    });
                });
                numberTictac++;

            }
            else if (timeInSecond == 4 && numberTictac == 1)
            {
                AudioManager.Play("tictac");
                clock.DORotate(new Vector3(0, 0, -6), 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            clock.DORotate(Vector3.zero, 0.05f).SetEase(Ease.OutSine);
                        });
                    });
                });
                numberTictac++;
            }
            else if (timeInSecond == 3 && numberTictac == 2)
            {
                AudioManager.Play("tictac");
                clock.DORotate(new Vector3(0, 0, -6), 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            clock.DORotate(Vector3.zero, 0.05f).SetEase(Ease.OutSine);
                        });
                    });
                });
                numberTictac++;
            }
            else if (timeInSecond == 2 && numberTictac == 3)
            {
                AudioManager.Play("tictac");
                clock.DORotate(new Vector3(0, 0, -6), 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            clock.DORotate(Vector3.zero, 0.05f).SetEase(Ease.OutSine);
                        });
                    });
                });
                numberTictac++;
            }
            else if (timeInSecond == 1 && numberTictac == 4)
            {
                AudioManager.Play("tictac");
                clock.DORotate(new Vector3(0, 0, -6), 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            clock.DORotate(Vector3.zero, 0.05f).SetEase(Ease.OutSine);
                        });
                    });
                });
                numberTictac++;
            }
            else if (timeInSecond == 0 && numberTictac == 5)
            {
                AudioManager.Play("tictac");
                clock.DORotate(new Vector3(0, 0, -6), 0.05f).SetEase(Ease.OutSine).OnComplete(() =>
                {
                    clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        clock.DORotate(new Vector3(0, 0, 6), 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            clock.DORotate(Vector3.zero, 0.05f).SetEase(Ease.OutSine);
                        });
                    });
                });
                numberTictac++;
            }
            if (numberPlaying + 1 <= 20)
            {
                sliceTime.fillAmount = time / 20f;
            }
            else if (numberPlaying + 1 > 20 && numberPlaying + 1 <= 50)
            {
                sliceTime.fillAmount = time / 25f;
            }
            else
            {
                sliceTime.fillAmount = time / 30f;
            }
        }
        else
        {
            if (losePanel.gameObject.activeInHierarchy) time = 0;
        }
        if (time >= 11)
        {
            txtTime.color = Color.black;
        }
        else
        {
            txtTime.color = Color.red;
        }
        timeInSecond = Mathf.FloorToInt(time);
        txtTime.text = timeInSecond.ToString();
        if (endGame.activeInHierarchy)
        {
            losePanel.gameObject.SetActive(false);
        }
        if (time <= 0 && !endGame.activeInHierarchy)
        {
            losePanel.gameObject.SetActive(true);
        }
        if (isWin)
        {
            buttonBack.DOAnchorPosX(-102, 0.3f).SetEase(Ease.OutQuart);
            buttonSuggest.DOAnchorPosX(260.2f, 0.3f).SetEase(Ease.OutQuart);
            buttonSkip.DOAnchorPosX(113.8f, 0.3f).SetEase(Ease.OutQuart);
            buttonTextLevel.DOAnchorPosY(115, 0.3f).SetEase(Ease.OutQuart);
            buttonTime.DOAnchorPosY(115, 0.3f).SetEase(Ease.OutQuart);
        }
        textLevel.text = "LEVEL " + (numberPlaying + 1).ToString();
    }
    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(7);
        startTime = true;
    }
    private void ChangeBG()
    {
        int numBG = Random.Range(0, background.Length);
        bg.sprite = background[numBG];
    }
    public void Back()
    {
        AudioManager.Play("click");
        if(unlockedLevelsNumber > GameManager.levelShowAd)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                StartCoroutine(EffectBack());
            });
        }
        else
        {
            StartCoroutine(EffectBack());
        }
    }
    IEnumerator EffectBack()
    {
        Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
        /*Level.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutQuart);
        buttonBack.DOAnchorPosX(-102, 0.75f).SetEase(Ease.OutQuart);
        buttonSuggest.DOAnchorPosX(260.2f, 0.75f).SetEase(Ease.OutQuart);
        buttonSkip.DOAnchorPosX(113.8f, 0.75f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(115, 0.75f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(115, 0.75f).SetEase(Ease.OutQuart);*/
        loading.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        home.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Setting()
    {
        AudioManager.Play("click");
        panelSetting.SetActive(true);
    }
    public void Skip()
    {
        AudioManager.Play("click");
        
        if(!PrefInfo.adEnable)
        {
            SkipAfterAds();
            return;
        }    
        MasterControl.Instance.ShowRewardAd((bool res) =>
        {
            if (res)
            {
                SkipAfterAds();
            }
        });

        void SkipAfterAds()
        {
            FirebaseManager.Instance.LogEvent("LEVEL_SKIP_" + (numberPlaying + 1));
            buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
            buttonSuggest.anchoredPosition = new Vector3(260.2f, buttonSuggest.anchoredPosition.y, 0);
            buttonSkip.anchoredPosition = new Vector3(113.8f, buttonSkip.anchoredPosition.y, 0);
            buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
            buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
            numberTictac = 0;
            if (numberPlaying == unlockedLevelsNumber - 1 && numberPlaying != GameManager.numberLevel - 1)
            {
                PlayerPrefs.SetInt("levelsUnlocked", unlockedLevelsNumber + 1);
            }
            Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
            if (Level != null)
            {
                Destroy(Level.gameObject);
            }
            if (numberPlaying != GameManager.numberLevel - 1)
            {
                numberPlaying++;
            }
            else
            {
                numberPlaying = 0;
            }
            if (unlockedLevelsNumber > 9 && unlockedLevelsNumber % 10 == 0 && unlockedModeNumber == 1)
            {
                trailerMode.SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                buttonBack.DOAnchorPosX(102, 0.8f).SetEase(Ease.OutQuart);
                buttonSuggest.DOAnchorPosX(-260.2f, 0.8f).SetEase(Ease.OutQuart);
                buttonSkip.DOAnchorPosX(-113.8f, 0.8f).SetEase(Ease.OutQuart);
                buttonTextLevel.DOAnchorPosY(-140, 0.8f).SetEase(Ease.OutQuart);
                buttonTime.DOAnchorPosY(-283, 0.8f).SetEase(Ease.OutQuart);
                GameObject loadedPrefab = Resources.Load<GameObject>(numberPlaying.ToString());
                GameObject level = Instantiate(loadedPrefab, gameObject.transform);
                level.transform.SetParent(gameObject.transform, false);
                if (numberPlaying + 1 <= 20)
                {
                    time = 20.5f;
                }
                else if (numberPlaying + 1 > 20 && numberPlaying + 1 <= 50)
                {
                    time = 25.5f;
                }
                else
                {
                    time = 30.5f;
                }
                startTime = false;
                AudioManager.Play("new_level");
            }
        }
    }
    public void Suggest()
    {
        AudioManager.Play("click");
        /*if (!PrefInfo.adEnable)
        {
            isHint = true;
            FirebaseManager.Instance.LogEvent("LEVEL_HINT_" + (numberPlaying + 1));
            return;
        }*/
        isHint = true;
        FirebaseManager.Instance.LogEvent("LEVEL_HINT_" + (numberPlaying + 1));
        /*MasterControl.Instance.ShowRewardAd((bool res) =>
        {
            if (res)
            {
                isHint = true;
                FirebaseManager.Instance.LogEvent("LEVEL_HINT_" + (numberPlaying + 1));
            }
        });*/
    }

    public void SkipLevel()
    {
        AudioManager.Play("click");
        if (!PrefInfo.adEnable)
        {
            SkipLevelAfterAds();
            return;
        }

        MasterControl.Instance.ShowRewardAd((bool res) =>
        {
            if (res )
            {
                SkipLevelAfterAds();
            }
        });

        void SkipLevelAfterAds()
        {
            numberTictac = 0;
            buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
            buttonSuggest.anchoredPosition = new Vector3(260.2f, buttonSuggest.anchoredPosition.y, 0);
            buttonSkip.anchoredPosition = new Vector3(113.8f, buttonSkip.anchoredPosition.y, 0);
            buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
            buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
            buttonBack.DOAnchorPosX(102, 0.8f).SetEase(Ease.OutQuart);
            buttonSuggest.DOAnchorPosX(-260.2f, 0.8f).SetEase(Ease.OutQuart);
            buttonSkip.DOAnchorPosX(-113.8f, 0.8f).SetEase(Ease.OutQuart);
            buttonTextLevel.DOAnchorPosY(-140, 0.8f).SetEase(Ease.OutQuart);
            buttonTime.DOAnchorPosY(-283, 0.8f).SetEase(Ease.OutQuart);
            if (numberPlaying == unlockedLevelsNumber - 1 && numberPlaying != GameManager.numberLevel - 1)
            {
                PlayerPrefs.SetInt("levelsUnlocked", unlockedLevelsNumber + 1);
            }
            Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
            if (Level != null)
            {
                Destroy(Level.gameObject);
            }
            if (numberPlaying != GameManager.numberLevel - 1)
            {
                numberPlaying++;
            }
            else
            {
                numberPlaying = 0;
            }
            StartCoroutine(ReloadLevel());
        }
    }
    public void Replay()
    {
        AudioManager.Play("click");
        if (unlockedLevelsNumber > GameManager.levelShowAd)
        {
            MasterControl.Instance.ShowInterAd((res) =>
            {
                numberTictac = 0;
                FirebaseManager.Instance.LogEvent("REPLAY_" + (numberPlaying + 1));
                Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
                if (Level != null)
                {
                    Destroy(Level.gameObject);
                }
                StartCoroutine(ReloadLevel());
            });
        }
        else
        {
            numberTictac = 0;
            FirebaseManager.Instance.LogEvent("REPLAY_" + (numberPlaying + 1));
            Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
            if (Level != null)
            {
                Destroy(Level.gameObject);
            }
            StartCoroutine(ReloadLevel());
        }
        
    }
    IEnumerator ReloadLevel()
    {
        losePanel.SetTrigger("hide");
        yield return new WaitForSeconds(0.5f);
        losePanel.gameObject.SetActive(false);
        GameObject loadedPrefab = Resources.Load<GameObject>(numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, gameObject.transform);
        level.transform.SetParent(gameObject.transform, false);
        if (numberPlaying + 1 <= 20)
        {
            time = 20.5f;
        }
        else if (numberPlaying + 1 > 20 && numberPlaying + 1 <= 50)
        {
            time = 25.5f;
        }
        else
        {
            time = 30.5f;
        }
        startTime = false;
        AudioManager.Play("new_level");
    }
    public void Home()
    {
        AudioManager.Play("click");
        if (unlockedLevelsNumber > GameManager.levelShowAd)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                StartCoroutine(EffectHome());
            });
        }
        else
        {
            StartCoroutine(EffectHome());
        }
    }
    IEnumerator EffectHome()
    {
        Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
        Level.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutQuart);
        buttonBack.DOAnchorPosX(-102, 0.75f).SetEase(Ease.OutQuart);
        buttonSuggest.DOAnchorPosX(260.2f, 0.75f).SetEase(Ease.OutQuart);
        buttonSkip.DOAnchorPosX(113.8f, 0.75f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(115, 0.75f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(115, 0.75f).SetEase(Ease.OutQuart);
        losePanel.SetTrigger("hide");
        yield return new WaitForSeconds(0.8f);
        losePanel.gameObject.SetActive(false);
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        home.SetActive(true);
        gameObject.SetActive(false);
    }
}
