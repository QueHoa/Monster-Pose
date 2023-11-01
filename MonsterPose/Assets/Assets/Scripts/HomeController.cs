using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;
using OneHit;

public class HomeController : MonoBehaviour
{
    [SerializeField]
    private Text textLevel;
    [SerializeField]
    private Text textPlayMode;
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
    public RectTransform buttonNoAds;
    public Button buttonPlay;
    public RectTransform buttonLevelList;
    public RectTransform buttonGallery;
    public Button buttonMode;
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

    private Tween butTween1;
    private Tween butTween2;
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
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonNoAds.anchoredPosition = new Vector3(123, buttonNoAds.anchoredPosition.y, 0);
        buttonPlay.interactable = true;
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        buttonMode.transform.localScale = new Vector3(0, 0, 1);
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        textLevel.text = "Level " + unlockedLevelsNumber.ToString();
        if (unlockedLevelsNumber > 10)
        {
            buttonMode.GetComponent<Button>().interactable = true;
            textLevelMode.gameObject.SetActive(true);
            textLevelMode.text = "Level " + unlockedModeNumber.ToString();
            textPlayMode.gameObject.SetActive(true);
            iconLockMode.gameObject.SetActive(false);
            textLockMode.gameObject.SetActive(false);
        }
        else
        {
            buttonMode.GetComponent<Button>().interactable = false;
            textPlayMode.gameObject.SetActive(false);
            textLevelMode.gameObject.SetActive(false);
            iconLockMode.gameObject.SetActive(true);
            textLockMode.gameObject.SetActive(true);
        }
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
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
        FirebaseManager.Instance.LogEvent("LEVEL_LEVELIST_ACCESS");
        StartCoroutine(EffectLevelList());
    }
    IEnumerator EffectLevelList()
    {
        AudioManager.Play("click");
        buttonSetting.DOAnchorPosX(-123, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(123, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        buttonMode.interactable = false;
        yield return new WaitForSeconds(0.4f);
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
        FirebaseManager.Instance.LogEvent("LEVEL_GALLERY_ACCESS");
        StartCoroutine(EffectGallery());
    }
    IEnumerator EffectGallery()
    {
        AudioManager.Play("click");
        buttonSetting.DOAnchorPosX(-123, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(123, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        buttonMode.interactable = false;
        yield return new WaitForSeconds(0.4f);
        gallery.SetActive(true);
        StartCoroutine(CreateGalleryWithNames());
        boardGallery.localScale = Vector3.one;
        buttonBackGallery.anchoredPosition = new Vector3(-112, buttonBack.anchoredPosition.y, 0);
        buttonTextGallery.anchoredPosition = new Vector3(buttonText.anchoredPosition.x, 160, 0);
        buttonBackGallery.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextGallery.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
    }
    public void Modelist()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_CHALLENGE_ACCESS");
        StartCoroutine(EffectMode());
    }
    IEnumerator EffectMode()
    {
        AudioManager.Play("click");
        buttonSetting.DOAnchorPosX(-123, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(123, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        buttonMode.interactable = false;
        yield return new WaitForSeconds(0.4f);
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
        buttonSetting.DOAnchorPosX(-123, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(123, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        buttonMode.interactable = false;
        yield return new WaitForSeconds(0.4f);
        GameObject loadedPrefab = Resources.Load<GameObject>((unlockedLevelsNumber - 1).ToString());
        GameObject level = Instantiate(loadedPrefab, main.transform);
        level.transform.SetParent(main.transform, false);
        mainController.numberPlaying = unlockedLevelsNumber - 1;
        main.SetActive(true);
        gameObject.SetActive(false);
    }
    IEnumerator StartHome()
    {
        buttonPlay.DOKill();
        buttonMode.DOKill();
        butTween1.Kill();
        butTween2.Kill();
        buttonPlay.transform.localScale = Vector3.zero;
        buttonMode.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.45f);
        buttonPlay.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        buttonMode.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.75f);
        butTween1 = buttonPlay.transform.DOScale(Vector3.one * 0.95f, 0.65f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        if (unlockedLevelsNumber >= 10)
        {
            butTween2 = buttonMode.transform.DOScale(Vector3.one * 0.95f, 0.65f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
    IEnumerator CreateButtonsWithNames()
    {
        for (int i = 0; i < numberLevel; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);
            newButton.name = i.ToString();
            if(i % 4 == 3)
            {
                yield return new WaitForSeconds(0.1f);
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
            if (i % 4 == 3)
            {
                yield return new WaitForSeconds(0.1f);
            }
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
                yield return new WaitForSeconds(0.5f);
            }
            if (!gallery.activeInHierarchy)
            {
                break;
            }
        }
    }
    public void SetBack()
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
        buttonBack.DOAnchorPosX(-112, 0.75f).SetEase(Ease.OutQuart);
        buttonText.DOAnchorPosY(160, 0.75f).SetEase(Ease.OutQuart);
        boardLevel.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.75f);
        buttonSetting.anchoredPosition = new Vector3(-123, buttonSetting.anchoredPosition.y, 0);
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonNoAds.anchoredPosition = new Vector3(123, buttonNoAds.anchoredPosition.y, 0);
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        buttonMode.transform.localScale = new Vector3(0, 0, 1);
        buttonPlay.interactable = true;
        if (unlockedLevelsNumber > 10)
        {
            buttonMode.GetComponent<Button>().interactable = true;
            textLevelMode.gameObject.SetActive(true);
            textLevelMode.text = "Level " + unlockedModeNumber.ToString();
            textPlayMode.gameObject.SetActive(true);
            iconLockMode.gameObject.SetActive(false);
            textLockMode.gameObject.SetActive(false);
        }
        else
        {
            buttonMode.GetComponent<Button>().interactable = false;
            textPlayMode.gameObject.SetActive(false);
            textLevelMode.gameObject.SetActive(false);
            iconLockMode.gameObject.SetActive(true);
            textLockMode.gameObject.SetActive(true);
        }
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
        buttonBackMode.DOAnchorPosX(-112, 0.75f).SetEase(Ease.OutQuart);
        buttonTextMode.DOAnchorPosY(160, 0.75f).SetEase(Ease.OutQuart);
        boardLevelMode.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.75f);
        buttonSetting.anchoredPosition = new Vector3(-123, buttonSetting.anchoredPosition.y, 0);
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonNoAds.anchoredPosition = new Vector3(123, buttonNoAds.anchoredPosition.y, 0);
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        buttonMode.transform.localScale = new Vector3(0, 0, 1);
        buttonPlay.interactable = true;
        if (unlockedLevelsNumber > 10)
        {
            buttonMode.GetComponent<Button>().interactable = true;
            textLevelMode.gameObject.SetActive(true);
            textLevelMode.text = "Level " + unlockedModeNumber.ToString();
            textPlayMode.gameObject.SetActive(true);
            iconLockMode.gameObject.SetActive(false);
            textLockMode.gameObject.SetActive(false);
        }
        else
        {
            buttonMode.GetComponent<Button>().interactable = false;
            textPlayMode.gameObject.SetActive(false);
            textLevelMode.gameObject.SetActive(false);
            iconLockMode.gameObject.SetActive(true);
            textLockMode.gameObject.SetActive(true);
        }
        levelListMode.SetActive(false);
        StartCoroutine(StartHome());
    }
    public void SetBackGallery()
    {
        AudioManager.Play("click");
        if(unlockedLevelsNumber > GameManager.levelShowAd)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                StartCoroutine(EffectBackGallery());
            });
        }
        else
        {
            StartCoroutine(EffectBackGallery());
        }
    }
    IEnumerator EffectBackGallery()
    {
        buttonBackGallery.DOAnchorPosX(-112, 0.75f).SetEase(Ease.OutQuart);
        buttonTextGallery.DOAnchorPosY(160, 0.75f).SetEase(Ease.OutQuart);
        boardGallery.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.75f);
        buttonSetting.anchoredPosition = new Vector3(-123, buttonSetting.anchoredPosition.y, 0);
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonNoAds.anchoredPosition = new Vector3(123, buttonNoAds.anchoredPosition.y, 0);
        buttonSetting.DOAnchorPosX(123, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosX(-123, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        buttonMode.transform.localScale = new Vector3(0, 0, 1);
        buttonPlay.interactable = true;
        if (unlockedLevelsNumber > 10)
        {
            buttonMode.GetComponent<Button>().interactable = true;
            textLevelMode.gameObject.SetActive(true);
            textLevelMode.text = "Level " + unlockedModeNumber.ToString();
            textPlayMode.gameObject.SetActive(true);
            iconLockMode.gameObject.SetActive(false);
            textLockMode.gameObject.SetActive(false);
        }
        else
        {
            buttonMode.GetComponent<Button>().interactable = false;
            textPlayMode.gameObject.SetActive(false);
            textLevelMode.gameObject.SetActive(false);
            iconLockMode.gameObject.SetActive(true);
            textLockMode.gameObject.SetActive(true);
        }
        gallery.SetActive(false);
        StartCoroutine(StartHome());
    }
    public void BackPhoto()
    {
        AudioManager.Play("click");
        if (unlockedLevelsNumber > GameManager.levelShowAd)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                StartCoroutine(EffectGallery1());
            });
        }
        else
        {
            StartCoroutine(EffectGallery1());
        }
    }
    IEnumerator EffectGallery1()
    {
        buttonBackPhoto.DOAnchorPosX(-112, 0.75f).SetEase(Ease.OutQuart);
        buttonTextPhoto.DOAnchorPosY(160, 0.75f).SetEase(Ease.OutQuart);
        imagePhoto.DOScale(Vector3.zero, 0.75f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.75f);
        photo.SetActive(false);
        boardGallery.DOScale(Vector3.one, 0.75f).SetEase(Ease.OutQuart);
        buttonBackGallery.anchoredPosition = new Vector3(-112, buttonBack.anchoredPosition.y, 0);
        buttonTextGallery.anchoredPosition = new Vector3(buttonText.anchoredPosition.x, 160, 0);
        buttonBackGallery.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextGallery.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
    }
}
