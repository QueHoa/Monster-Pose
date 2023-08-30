using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;

public class ChooseLevelMode : MonoBehaviour
{
    [SerializeField]
    private Text numberLevel;
    [SerializeField]
    private Image BG;
    [SerializeField]
    public Sprite[] colorBG;
    [SerializeField]
    private GameObject tickDone;

    private GameObject mode;
    private GameObject home;
    private GameObject levelListMode;
    private ModeController modeController;
    private HomeController homeController;
    private int unlockedModeNumber;
    private int value;
    void Start()
    {
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        transform.localScale = new Vector3(0, 0, 1);
        mode = GameManager.Instance.mode;
        home = GameManager.Instance.home;
        levelListMode = GameManager.Instance.levelListMode;
        modeController = GameManager.Instance.modeController;
        homeController = GameManager.Instance.homeController;
        value = int.Parse(gameObject.name) ;
        numberLevel.text = value.ToString();
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuart);
        if (value <= unlockedModeNumber)
        {
            gameObject.GetComponent<Button>().interactable = true;
            if (value == unlockedModeNumber)
            {
                BG.sprite = colorBG[1];
                tickDone.SetActive(true);
            }
            else
            {
                BG.sprite = colorBG[0];
                tickDone.SetActive(false);
            }

        }
        else
        {
            BG.sprite = colorBG[2];
            tickDone.SetActive(false);
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    void Update()
    {
        
    }

    public void Choose()
    {
        StartCoroutine(EffectChoose());
    }
    IEnumerator EffectChoose()
    {
        AudioManager.Play("click");
        homeController.buttonBackMode.DOAnchorPosX(-112, 0.5f).SetEase(Ease.OutQuart);
        homeController.buttonTextMode.DOAnchorPosY(160, 0.5f).SetEase(Ease.OutQuart);
        homeController.boardLevelMode.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        modeController.numberPlaying = int.Parse(gameObject.name);
        GameObject loadedPrefab = Resources.Load<GameObject>("Lv" + gameObject.name);
        GameObject level = Instantiate(loadedPrefab, mode.transform);
        level.transform.SetParent(mode.transform, false);
        levelListMode.SetActive(false);
        mode.SetActive(true);
        home.SetActive(false);
    }
}
