using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;

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
    [SerializeField]
    private Image sliceTime;
    [SerializeField]
    private Text txtTime;
    [SerializeField]
    private GameObject home;
    [SerializeField]
    private GameObject losePanel;
    [SerializeField]
    private GameObject endGame;
    public RectTransform buttonBack;
    public RectTransform buttonSuggest;
    public RectTransform buttonSkip;
    public RectTransform buttonTextLevel;
    public RectTransform buttonTime;
    private float time;
    private int timeInSecond;
    private int unlockedLevelsNumber;

    // Start is called before the first frame update
    void Start()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        numberPlaying = unlockedLevelsNumber - 1;
    }
    private void OnEnable()
    {
        buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
        buttonSuggest.anchoredPosition = new Vector3(260.2f, buttonSuggest.anchoredPosition.y, 0);
        buttonSkip.anchoredPosition = new Vector3(113.8f, buttonSkip.anchoredPosition.y, 0);
        buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
        buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
        time = 30.5f;
        sliceTime.fillAmount = 1;
        ChangeBG();
        AudioManager.Play("new_level");
        isHint = false;
        isWin = false;
        buttonBack.DOAnchorPosX(102, 0.5f).SetEase(Ease.OutQuart);
        buttonSuggest.DOAnchorPosX(-260.2f, 0.5f).SetEase(Ease.OutQuart);
        buttonSkip.DOAnchorPosX(-113.8f, 0.5f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(-115, 0.5f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(-283, 0.5f).SetEase(Ease.OutQuart);
    }
    // Update is called once per frame
    void Update()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        if (!losePanel.activeInHierarchy && !endGame.activeInHierarchy)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
            }
            sliceTime.fillAmount = time / 30f;
        }
        else
        {
            if(losePanel.activeInHierarchy) time = 0;
        }
        if(time >= 11)
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
            losePanel.SetActive(false);
        }
        if(time <= 0 && !endGame.activeInHierarchy)
        {
            losePanel.SetActive(true);
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
    private void ChangeBG()
    {
        int numBG = Random.Range(0, background.Length);
        bg.sprite = background[numBG];
    }
    public void Back()
    {
        StartCoroutine(EffectBack());
    }
    IEnumerator EffectBack()
    {
        AudioManager.Play("click");
        Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
        Level.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuart);
        buttonBack.DOAnchorPosX(-102, 0.3f).SetEase(Ease.OutQuart);
        buttonSuggest.DOAnchorPosX(260.2f, 0.3f).SetEase(Ease.OutQuart);
        buttonSkip.DOAnchorPosX(113.8f, 0.3f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(115, 0.3f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(115, 0.3f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        home.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Skip()
    {
        AudioManager.Play("click");
        buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
        buttonSuggest.anchoredPosition = new Vector3(260.2f, buttonSuggest.anchoredPosition.y, 0);
        buttonSkip.anchoredPosition = new Vector3(113.8f, buttonSkip.anchoredPosition.y, 0);
        buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
        buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
        buttonBack.DOAnchorPosX(102, 0.5f).SetEase(Ease.OutQuart);
        buttonSuggest.DOAnchorPosX(-260.2f, 0.5f).SetEase(Ease.OutQuart);
        buttonSkip.DOAnchorPosX(-113.8f, 0.5f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(-115, 0.5f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(-283, 0.5f).SetEase(Ease.OutQuart);
        if (numberPlaying == unlockedLevelsNumber - 1)
        {
            PlayerPrefs.SetInt("levelsUnlocked", unlockedLevelsNumber + 1);
        }
        Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        numberPlaying++;
        GameObject loadedPrefab = Resources.Load<GameObject>(numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, gameObject.transform);
        level.transform.SetParent(gameObject.transform, false);
        time = 30;
        AudioManager.Play("new_level");
    }
    public void Suggest()
    {
        AudioManager.Play("click");
        isHint = true;
    }
    
    public void SkipLevel()
    {
        AudioManager.Play("click");
        buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
        buttonSuggest.anchoredPosition = new Vector3(260.2f, buttonSuggest.anchoredPosition.y, 0);
        buttonSkip.anchoredPosition = new Vector3(113.8f, buttonSkip.anchoredPosition.y, 0);
        buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
        buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
        buttonBack.DOAnchorPosX(102, 0.5f).SetEase(Ease.OutQuart);
        buttonSuggest.DOAnchorPosX(-260.2f, 0.5f).SetEase(Ease.OutQuart);
        buttonSkip.DOAnchorPosX(-113.8f, 0.5f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(-115, 0.5f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(-283, 0.5f).SetEase(Ease.OutQuart);
        if (numberPlaying == unlockedLevelsNumber - 1)
        {
            PlayerPrefs.SetInt("levelsUnlocked", unlockedLevelsNumber + 1);
        }
        Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        numberPlaying++;
        losePanel.SetActive(false);
        GameObject loadedPrefab = Resources.Load<GameObject>(numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, gameObject.transform);
        level.transform.SetParent(gameObject.transform, false);
        time = 30;
        AudioManager.Play("new_level");

    }
    public void Replay()
    {
        AudioManager.Play("click");
        Transform Level = transform.Find(numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        losePanel.SetActive(false);
        GameObject loadedPrefab = Resources.Load<GameObject>(numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, gameObject.transform);
        level.transform.SetParent(gameObject.transform, false);
        time = 30;
        AudioManager.Play("new_level");
    }
}
