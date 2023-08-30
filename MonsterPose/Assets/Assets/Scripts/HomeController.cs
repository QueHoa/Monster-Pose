using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;

public class HomeController : MonoBehaviour
{
    [SerializeField]
    private Text textLevel;
    [SerializeField]
    private Text textLevelMode;
    [SerializeField]
    private Text textLockMode;
    [SerializeField]
    private Image iconLockMode;
    public GameObject main;
    public GameObject panelSetting;
    public GameObject levelList;
    public GameObject levelListMode;
    public GameObject gallery;
    public GameObject photo;
    public GameObject mode;
    public RectTransform buttonSetting;
    public RectTransform buttonPlay;
    public RectTransform buttonLevelList;
    public RectTransform buttonGallery;
    public RectTransform buttonMode;
    public RectTransform buttonBack;
    public RectTransform buttonText;
    public RectTransform buttonBackMode;
    public RectTransform buttonTextMode;
    public RectTransform buttonBackGallery;
    public RectTransform buttonTextGallery;
    public RectTransform buttonBackPhoto;
    public RectTransform buttonTextPhoto;
    public RectTransform imagePhoto;
    public GameObject buttonPrefab;
    public GameObject buttonModePrefab;
    public GameObject buttonGalleryPrefab;
    public Transform boardLevel;
    public Transform boardLevelMode;
    public Transform boardGallery;
    public Transform buttonParent;
    public Transform buttonModeParent;
    public Transform buttonGalleryParent;
    public int numberLevel;
    public int numberMode;

    private int unlockedLevelsNumber;
    private int unlockedModeNumber;
    private MainController mainController;

    // Start is called before the first frame update
    void Start()
    {
        mainController = GameManager.Instance.mainController;
    }
    private void OnEnable()
    {
        buttonSetting.anchoredPosition = new Vector3(-123, buttonSetting.anchoredPosition.y, 0);
        buttonPlay.localScale = new Vector3(0, 0, 1);
        buttonLevelList.localScale = new Vector3(0, 0, 1);
        buttonGallery.localScale = new Vector3(0, 0, 1);
        buttonMode.localScale = new Vector3(0, 0, 1);
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        textLevel.text = "Level " + unlockedLevelsNumber.ToString();
        if(unlockedLevelsNumber >= 10)
        {
            buttonMode.GetComponent<Button>().interactable = true;
            textLevelMode.gameObject.SetActive(true);
            textLevelMode.text = "Level " + unlockedModeNumber.ToString();
            iconLockMode.gameObject.SetActive(false);
            textLockMode.gameObject.SetActive(false);
        }
        else
        {
            buttonMode.GetComponent<Button>().interactable = false;
            textLevelMode.gameObject.SetActive(false);
            iconLockMode.gameObject.SetActive(true);
            textLockMode.gameObject.SetActive(true);
        }
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        StartCoroutine(StartHome());
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelList.activeInHierarchy)
        {
            Transform[] children = new Transform[buttonParent.childCount];
            for (int i = 0; i < buttonParent.childCount; i++)
            {
                children[i] = buttonParent.GetChild(i);
            }
            foreach (Transform child in children)
            {
                Destroy(child.gameObject);
            }
        }
        if (!levelListMode.activeInHierarchy)
        {
            Transform[] children = new Transform[buttonModeParent.childCount];
            for (int i = 0; i < buttonModeParent.childCount; i++)
            {
                children[i] = buttonModeParent.GetChild(i);
            }
            foreach (Transform child in children)
            {
                Destroy(child.gameObject);
            }
        }
        if (!gallery.activeInHierarchy)
        {
            Transform[] children = new Transform[buttonGalleryParent.childCount];
            for (int i = 0; i < buttonGalleryParent.childCount; i++)
            {
                children[i] = buttonGalleryParent.GetChild(i);
            }
            foreach (Transform child in children)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void Setting()
    {
        AudioManager.Play("click");
        panelSetting.SetActive(true);
    }
    public void LevelList()
    {
        StartCoroutine(EffectLevelList());
    }
    IEnumerator EffectLevelList()
    {
        AudioManager.Play("click");
        buttonSetting.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonLevelList.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonGallery.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonMode.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        levelList.SetActive(true);
        boardLevel.localScale = Vector3.one;
        StartCoroutine(CreateButtonsWithNames());
        buttonBack.anchoredPosition = new Vector3(-112, buttonBack.anchoredPosition.y, 0);
        buttonText.anchoredPosition = new Vector3(buttonText.anchoredPosition.x, 160, 0);
        buttonBack.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        buttonText.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
    }
    public void Gallery()
    {
        StartCoroutine(EffectGallery());
    }
    IEnumerator EffectGallery()
    {
        AudioManager.Play("click");
        buttonSetting.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonLevelList.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonGallery.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonMode.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.3f);
        gallery.SetActive(true);
        boardGallery.localScale = Vector3.one;
        StartCoroutine(CreateGalleryWithNames());
        buttonBackGallery.anchoredPosition = new Vector3(-112, buttonBack.anchoredPosition.y, 0);
        buttonTextGallery.anchoredPosition = new Vector3(buttonText.anchoredPosition.x, 160, 0);
        buttonBackGallery.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextGallery.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
    }
    public void Modelist()
    {
        StartCoroutine(EffectMode());
    }
    IEnumerator EffectMode()
    {
        AudioManager.Play("click");
        buttonSetting.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonLevelList.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonGallery.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonMode.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        levelListMode.SetActive(true);
        boardLevelMode.localScale = Vector3.one;
        StartCoroutine(CreateButtonsModeWithNames());
        buttonBackMode.anchoredPosition = new Vector3(-112, buttonBackMode.anchoredPosition.y, 0);
        buttonTextMode.anchoredPosition = new Vector3(buttonTextMode.anchoredPosition.x, 160, 0);
        buttonBackMode.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextMode.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
    }
    public void Play()
    {
        StartCoroutine(EffectPlay());
    }
    IEnumerator EffectPlay()
    {
        AudioManager.Play("click");
        buttonSetting.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonLevelList.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonGallery.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        buttonMode.DOScale(0, 0.6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        GameObject loadedPrefab = Resources.Load<GameObject>((unlockedLevelsNumber - 1).ToString());
        GameObject level = Instantiate(loadedPrefab, main.transform);
        level.transform.SetParent(main.transform, false);
        mainController.numberPlaying = unlockedLevelsNumber - 1;
        main.SetActive(true);
        gameObject.SetActive(false);
    }
    IEnumerator StartHome()
    {
        yield return new WaitForSeconds(0.6f);
        buttonPlay.DOScale(1, 0.6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.4f);
        buttonLevelList.DOScale(1, 0.6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.4f);
        buttonGallery.DOScale(1, 0.6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.4f);
        buttonMode.DOScale(1, 0.6f).SetEase(Ease.OutQuart);
    }
    IEnumerator CreateButtonsWithNames()
    {
        for (int i = 0; i < numberLevel; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.name = i.ToString();
            if(i % 4 == 3)
            {
                yield return new WaitForSeconds(0.25f);
            }
            if (!levelList.activeInHierarchy)
            {
                break;
            }
        }
    }
    IEnumerator CreateButtonsModeWithNames()
    {
        for (int i = 0; i < numberMode; i++)
        {
            GameObject newButton = Instantiate(buttonModePrefab, buttonModeParent);
            newButton.name = (i+1).ToString();
            yield return new WaitForSeconds(0.25f);
            if (!levelListMode.activeInHierarchy)
            {
                break;
            }
        }
    }
    IEnumerator CreateGalleryWithNames()
    {
        for (int i = 0; i < numberLevel; i++)
        {
            GameObject newButton = Instantiate(buttonGalleryPrefab, buttonGalleryParent);
            newButton.name = i.ToString();
            if (i % 2 == 1)
            {
                yield return new WaitForSeconds(0.4f);
            }
            if (!gallery.activeInHierarchy)
            {
                break;
            }
        }
    }
    public void SetBack()
    {
        StartCoroutine(EffectBack());
    }
    IEnumerator EffectBack()
    {
        AudioManager.Play("click");
        buttonBack.DOAnchorPosX(-112, 0.5f).SetEase(Ease.OutQuart);
        buttonText.DOAnchorPosY(160, 0.5f).SetEase(Ease.OutQuart);
        boardLevel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        buttonSetting.anchoredPosition = new Vector3(-123, buttonSetting.anchoredPosition.y, 0);
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.localScale = new Vector3(0, 0, 1);
        buttonLevelList.localScale = new Vector3(0, 0, 1);
        buttonGallery.localScale = new Vector3(0, 0, 1);
        buttonMode.localScale = new Vector3(0, 0, 1);
        levelList.SetActive(false);
        StartCoroutine(StartHome());
    }
    public void SetBackMode()
    {
        StartCoroutine(EffectBackMode());
    }
    IEnumerator EffectBackMode()
    {
        AudioManager.Play("click");
        buttonBackMode.DOAnchorPosX(-112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextMode.DOAnchorPosY(160, 0.5f).SetEase(Ease.OutQuart);
        boardLevelMode.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        buttonSetting.anchoredPosition = new Vector3(-123, buttonSetting.anchoredPosition.y, 0);
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.localScale = new Vector3(0, 0, 1);
        buttonLevelList.localScale = new Vector3(0, 0, 1);
        buttonGallery.localScale = new Vector3(0, 0, 1);
        buttonMode.localScale = new Vector3(0, 0, 1);
        levelListMode.SetActive(false);
        StartCoroutine(StartHome());
    }
    public void SetBackGallery()
    {
        StartCoroutine(EffectBackGallery());
    }
    IEnumerator EffectBackGallery()
    {
        AudioManager.Play("click");
        buttonBackGallery.DOAnchorPosX(-112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextGallery.DOAnchorPosY(160, 0.5f).SetEase(Ease.OutQuart);
        boardGallery.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        buttonSetting.anchoredPosition = new Vector3(-123, buttonSetting.anchoredPosition.y, 0);
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.localScale = new Vector3(0, 0, 1);
        buttonLevelList.localScale = new Vector3(0, 0, 1);
        buttonGallery.localScale = new Vector3(0, 0, 1);
        buttonMode.localScale = new Vector3(0, 0, 1);
        gallery.SetActive(false);
        StartCoroutine(StartHome());
    }
    public void BackPhoto()
    {
        StartCoroutine(EffectGallery1());
    }
    IEnumerator EffectGallery1()
    {
        AudioManager.Play("click");
        buttonBackPhoto.DOAnchorPosX(-112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextPhoto.DOAnchorPosY(160, 0.5f).SetEase(Ease.OutQuart);
        imagePhoto.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        gallery.SetActive(true);
        photo.SetActive(false);
        boardGallery.localScale = Vector3.one;
        StartCoroutine(CreateGalleryWithNames());
        buttonBackGallery.anchoredPosition = new Vector3(-112, buttonBack.anchoredPosition.y, 0);
        buttonTextGallery.anchoredPosition = new Vector3(buttonText.anchoredPosition.x, 160, 0);
        buttonBackGallery.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextGallery.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
    }
}
