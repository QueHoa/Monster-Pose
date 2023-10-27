using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;
using OneHit;

public class ModeController : MonoBehaviour
{
    [HideInInspector]
    public int numberPlaying;
    [HideInInspector]
    public bool isWin;
    public Animator lose;
    public Image bg;
    public Sprite[] background;
    public GameObject home;
    public GameObject loseGame;
    public GameObject endGame;
    public RectTransform buttonBack;
    public RectTransform buttonTextLevel;
    public RectTransform buttonTime;

    public Transform clock;
    [SerializeField]
    private Image sliceTime;
    [SerializeField]
    private Text txtTime;
    private float time;
    private int timeInSecond;
    private int unlockedModeNumber;
    private int numberTictac;
    private bool startTime;
    // Start is called before the first frame update
    void Start()
    {
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
    }
    private void OnEnable()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_CHALLENGE_REPLAY_" + numberPlaying);
        buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
        buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
        buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
        loseGame.SetActive(false);
        if (numberPlaying <= 20)
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
        isWin = false;
        buttonBack.DOAnchorPosX(102, 0.5f).SetEase(Ease.OutQuart);        
        buttonTextLevel.DOAnchorPosY(-115, 0.5f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(-260, 0.5f).SetEase(Ease.OutQuart);
        startTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!loseGame.activeInHierarchy && !endGame.activeInHierarchy)
        {
            if (numberPlaying != 1) 
            {
                startTime = true;
            }
            else
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
            if (numberPlaying <= 20)
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
            if (loseGame.activeInHierarchy) time = 0;
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
            loseGame.SetActive(false);
        }
        if (time <= 0 && !endGame.activeInHierarchy && !loseGame.activeInHierarchy)
        {
            loseGame.SetActive(true);
        }
        if (isWin)
        {
            buttonBack.DOAnchorPosX(-102, 0.3f).SetEase(Ease.OutQuart);
            buttonTextLevel.DOAnchorPosY(115, 0.3f).SetEase(Ease.OutQuart);
            buttonTime.DOAnchorPosY(115, 0.3f).SetEase(Ease.OutQuart);
        }
    }
    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(14);
        startTime = true;
    }
    public void Back()
    {
        AudioManager.Play("click");
        MasterControl.Instance.ShowInterAd((bool res) =>
        {
            StartCoroutine(EffectBack());
        });
        
    }
    IEnumerator EffectBack()
    {
        Transform Level = transform.Find("Lv" + numberPlaying.ToString() + "(Clone)");
        Level.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutQuart);
        buttonBack.DOAnchorPosX(-102, 0.75f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(115, 0.75f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(115, 0.75f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.8f);
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        home.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Replay()
    {
        AudioManager.Play("click");
        if (numberPlaying <= 3)
        {
            StartCoroutine(effectReplay());
        }
        else
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                StartCoroutine(effectReplay());
            });
        }
    }
    IEnumerator effectReplay()
    {
        lose.SetTrigger("hide");
        yield return new WaitForSeconds(0.5f);
        Transform Level = transform.Find("Lv" + numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        numberTictac = 0;
        loseGame.SetActive(false);
        GameObject loadedPrefab = Resources.Load<GameObject>("Lv" + numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, gameObject.transform);
        level.transform.SetParent(gameObject.transform, false);
        if (numberPlaying <= 20)
        {
            time = 25.5f;
        }
        else 
        {
            time = 30.5f;
        }
        AudioManager.Play("new_level");
    }
    public void Home()
    {
        AudioManager.Play("click");
        MasterControl.Instance.ShowInterAd((bool res) =>
        {
            isWin = false;
            Transform Level = transform.Find("Lv" + numberPlaying.ToString() + "(Clone)");
            if (Level != null)
            {
                Destroy(Level.gameObject);
            }
            StartCoroutine(HideHome());
        });
    }
    IEnumerator HideHome()
    {
        lose.SetTrigger("hide");
        yield return new WaitForSeconds(0.5f);
        loseGame.SetActive(false);
        home.SetActive(true);
        //yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }
    private void ChangeBG()
    {
        int numBG = Random.Range(0, background.Length);
        bg.sprite = background[numBG];
    }
}
