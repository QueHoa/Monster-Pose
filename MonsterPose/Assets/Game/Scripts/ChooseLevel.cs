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
    private GameObject loading;
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
        loading = GameManager.Instance.loading;
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
        /*homeController.buttonBack.DOAnchorPosX(-112, 0.75f).SetEase(Ease.OutQuart);
        homeController.buttonText.DOAnchorPosY(160, 0.75f).SetEase(Ease.OutQuart);
        homeController.boardLevel.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutQuart);*/
        loading.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        mainController.numberPlaying = int.Parse(gameObject.name);
        if (mainController.numberPlaying == 0)
        {
            if (GameManager.numberMonster == 1)
            {
                GameObject loadedPrefab = Resources.Load<GameObject>("01");
                GameObject level = Instantiate(loadedPrefab, main.transform);
                level.transform.SetParent(main.transform, false);
            }
            else if (GameManager.numberMonster == 2)
            {
                GameObject loadedPrefab = Resources.Load<GameObject>("02");
                GameObject level = Instantiate(loadedPrefab, main.transform);
                level.transform.SetParent(main.transform, false);
            }
            else if (GameManager.numberMonster == 3)
            {
                GameObject loadedPrefab = Resources.Load<GameObject>("03");
                GameObject level = Instantiate(loadedPrefab, main.transform);
                level.transform.SetParent(main.transform, false);
            }
        }
        else
        {
            GameObject loadedPrefab = Resources.Load<GameObject>(gameObject.name);
            GameObject level = Instantiate(loadedPrefab, main.transform);
            level.transform.SetParent(main.transform, false);
        }
        levelList.SetActive(false);
        main.SetActive(true);
        home.SetActive(false);
    }
}
