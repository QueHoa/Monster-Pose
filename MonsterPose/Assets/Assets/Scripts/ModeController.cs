using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;

public class ModeController : MonoBehaviour
{
    [HideInInspector]
    public int numberPlaying;
    [HideInInspector]
    public bool isWin;
    public GameObject home;
    public GameObject loseGame;
    public GameObject endGame;
    public RectTransform buttonBack;
    public RectTransform buttonTextLevel;
    public RectTransform buttonTime;

    [SerializeField]
    private Image sliceTime;
    [SerializeField]
    private Text txtTime;
    private float time;
    private int timeInSecond;
    private int unlockedModeNumber;
    // Start is called before the first frame update
    void Start()
    {
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
    }
    private void OnEnable()
    {
        buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
        buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
        buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
        time = 30.5f;
        isWin = false;
        buttonBack.DOAnchorPosX(102, 0.5f).SetEase(Ease.OutQuart);        
        buttonTextLevel.DOAnchorPosY(-115, 0.5f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(-283, 0.5f).SetEase(Ease.OutQuart);
    }

    // Update is called once per frame
    void Update()
    {
        if (!loseGame.activeInHierarchy && !endGame.activeInHierarchy)
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
        if (time <= 0 && !endGame.activeInHierarchy)
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
    public void Back()
    {
        StartCoroutine(EffectBack());
    }
    IEnumerator EffectBack()
    {
        AudioManager.Play("click");
        Transform Level = transform.Find("Lv" + numberPlaying.ToString() + "(Clone)");
        Level.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutQuart);
        buttonBack.DOAnchorPosX(-102, 0.3f).SetEase(Ease.OutQuart);
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
    public void Replay()
    {
        AudioManager.Play("click");
        Transform Level = transform.Find("Lv" + numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        loseGame.SetActive(false);
        GameObject loadedPrefab = Resources.Load<GameObject>("Lv" + numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, gameObject.transform);
        level.transform.SetParent(gameObject.transform, false);
        time = 30;
        AudioManager.Play("new_level");
    }
    public void SkipLevel()
    {
        AudioManager.Play("click");
        buttonBack.anchoredPosition = new Vector3(-102, buttonBack.anchoredPosition.y, 0);
        buttonTextLevel.anchoredPosition = new Vector3(buttonTextLevel.anchoredPosition.x, 115, 0);
        buttonTime.anchoredPosition = new Vector3(buttonTime.anchoredPosition.x, 115, 0);
        buttonBack.DOAnchorPosX(102, 0.5f).SetEase(Ease.OutQuart);
        buttonTextLevel.DOAnchorPosY(-115, 0.5f).SetEase(Ease.OutQuart);
        buttonTime.DOAnchorPosY(-283, 0.5f).SetEase(Ease.OutQuart);
        if (numberPlaying == unlockedModeNumber - 1)
        {
            PlayerPrefs.SetInt("levelsUnlocked", unlockedModeNumber + 1);
        }
        Transform Level = transform.Find( "Lv" + numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        numberPlaying++;
        loseGame.SetActive(false);
        GameObject loadedPrefab = Resources.Load<GameObject>("Lv" + numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, gameObject.transform);
        level.transform.SetParent(gameObject.transform, false);
        time = 30;
        AudioManager.Play("new_level");

    }
}
