using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;
using OneHit;
public class ChooseLevel : MonoBehaviour
{
    [SerializeField]
    private Text numberLevel;
    [SerializeField]
    private Image BG;
    [SerializeField]
    public Sprite[] colorBG;
    [SerializeField]
    private GameObject tickDone;

    private GameObject main;
    private GameObject home;
    private GameObject levelList;
    private MainController mainController;
    private HomeController homeController;
    private int unlockedLevelsNumber;
    private int value;
    void Start()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        transform.localScale = new Vector3(0, 0, 1);
        main = GameManager.Instance.main;
        home = GameManager.Instance.home;
        levelList = GameManager.Instance.levelList;
        mainController = GameManager.Instance.mainController;
        homeController = GameManager.Instance.homeController;
        value = int.Parse(gameObject.name);
        value += 1;
        numberLevel.text = value.ToString();
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuart);
        if (value <= unlockedLevelsNumber)
        {
            gameObject.GetComponent<Button>().interactable = true;
            if (value == unlockedLevelsNumber)
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
        homeController.buttonBack.DOAnchorPosX(-112, 0.75f).SetEase(Ease.OutQuart);
        homeController.buttonText.DOAnchorPosY(160, 0.75f).SetEase(Ease.OutQuart);
        homeController.boardLevel.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.8f);
        mainController.numberPlaying = int.Parse(gameObject.name);
        GameObject loadedPrefab = Resources.Load<GameObject>(gameObject.name);
        GameObject level = Instantiate(loadedPrefab, main.transform);
        level.transform.SetParent(main.transform, false);
        levelList.SetActive(false);
        main.SetActive(true);
        home.SetActive(false);
    }
}
