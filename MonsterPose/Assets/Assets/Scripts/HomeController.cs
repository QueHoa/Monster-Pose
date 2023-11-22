using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit.Framework;
using OneHit;
using Dan.Main;
using System.Security.Cryptography.X509Certificates;

public class HomeController : MonoBehaviour
{
    public UIPopupName popupName;
    
    [Header("GameObject:")]
    public GameObject iconLockMode;
    public GameObject iconUnlockMode;
    public GameObject main;
    public GameObject panelSetting;
    public GameObject panelProfile;
    public GameObject levelList;
    public GameObject levelListMode;
    public GameObject gallery;
    public GameObject leaderBoard;
    public GameObject photo;
    public GameObject mode;
    public GameObject loading;
    public GameObject buttonPrefab;
    public GameObject buttonModePrefab;
    public GameObject buttonGalleryPrefab;

    [Header("Tranform:")]
    public RectTransform buttonSetting;
    public RectTransform buttonProfile;
    public RectTransform buttonNoAds;
    public Button buttonPlay;
    public RectTransform buttonLevelList;
    public RectTransform buttonGallery;
    public RectTransform buttonLeaderboard;
    public RectTransform buttonMode;
    public Button btnMode;
    public RectTransform buttonBack;
    public RectTransform buttonText;
    public RectTransform buttonBackMode;
    public RectTransform buttonTextMode;
    public RectTransform buttonBackGallery;
    public RectTransform buttonTextGallery;
    public RectTransform buttonBackPhoto;
    public RectTransform buttonTextPhoto;
    public RectTransform imagePhoto;
    public Transform boardLevel;
    public Transform boardLevelMode;
    public Transform boardGallery;
    public Transform buttonParent;
    public Transform buttonModeParent;
    public Transform buttonGalleryParent;

    [Header("UI:")]
    public Text textLevel;
    public Text textPlayMode;
    public Image avatar;
    public Text nameProfile;
    public Text score;
    public Sprite[] listAvt;
    public int numberLevel;
    public int numberMode;

    private Tween butTween1;
    private int unlockedLevelsNumber;
    private int unlockedModeNumber;
    private MainController mainController;
    private string nameUser;
    private int yourScore;
    private int idAvt;

    void Awake()
    {
        mainController = GameManager.Instance.mainController;
        if (!PlayerPrefs.HasKey("name"))
        {
            PlayerPrefs.SetString("name", "");
        }
        if (!PlayerPrefs.HasKey("avatar"))
        {
            PlayerPrefs.SetInt("avatar", Random.Range(0,7));
        }
        if (!PlayerPrefs.HasKey("score"))
        {
            PlayerPrefs.SetInt("score", 0);
        }
    }
    private void OnEnable()
    {
        buttonSetting.anchoredPosition = new Vector3(buttonSetting.anchoredPosition.x, 150, 0);
        buttonProfile.anchoredPosition = new Vector3(buttonProfile.anchoredPosition.x, 150, 0);
        buttonNoAds.anchoredPosition = new Vector3(buttonNoAds.anchoredPosition.x, 150, 0);
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonMode.anchoredPosition = new Vector3(-130, buttonMode.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonLeaderboard.anchoredPosition = new Vector3(130, buttonLeaderboard.anchoredPosition.y, 0);
        buttonPlay.interactable = true;
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        textLevel.transform.localScale = new Vector3(0, 0, 1);
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        idAvt = PlayerPrefs.GetInt("avatar");
        if(idAvt >= 0 && idAvt < 8)
        {
            avatar.sprite = listAvt[idAvt];
        }
        textLevel.text = "LEVEL " + unlockedLevelsNumber.ToString();
        if(PlayerPrefs.GetString("name").Length >=3 && PlayerPrefs.GetString("name").Length <= 10)
        {
            LeaderboardCreator.UploadNewEntry(LeaderBoardMain.publicKey,
                    PlayerPrefs.GetString("name"),
                    PlayerPrefs.GetInt("score"),
                    PlayerPrefs.GetInt("avatar").ToString()
                    );
        }
        yourScore = 0;
        for (int i = 0; i < unlockedLevelsNumber; i++)
        {
            yourScore += PlayerPrefs.GetInt(i.ToString());
            PlayerPrefs.SetInt("score", yourScore);
        }
        if (unlockedLevelsNumber > 10)
        {
            btnMode.interactable = true;
            textPlayMode.text = "CHALLENGE";
            iconLockMode.SetActive(false);
            iconUnlockMode.SetActive(true);
        }
        else
        {
            btnMode.interactable = false;
            textPlayMode.text = "UNLOCK LEVEL 10";
            iconLockMode.SetActive(true);
            iconUnlockMode.SetActive(false);
        }
        buttonProfile.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        StartCoroutine(StartHome());
    }

    // Update is called once per frame
    void Update()
    {
        nameUser = PlayerPrefs.GetString("name");
        
        if(nameUser.Length >= 3 && nameUser.Length <= 10)
        {
            nameProfile.text = nameUser;
        }
        score.text = yourScore.ToString();
        idAvt = PlayerPrefs.GetInt("avatar");
        if (idAvt >= 0 && idAvt < 8)
        {
            avatar.sprite = listAvt[idAvt];
        }
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
    public void Profile()
    {
        AudioManager.Play("click");
        if (PlayerPrefs.GetString("name") == "")
        {
            popupName.gameObject.SetActive(true);
            popupName.idProfile = 1;
        }
        else
        {
            FirebaseManager.Instance.LogEvent("PROFILE_ACCESS");
            panelProfile.SetActive(true);
        }
    }
    public void LevelList()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_LEVELIST_ACCESS");
        StartCoroutine(EffectLevelList());
    }
    IEnumerator EffectLevelList()
    {
        AudioManager.Play("click");
        buttonProfile.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        textLevel.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        btnMode.interactable = false;
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
        buttonProfile.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        textLevel.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        btnMode.interactable = false;
        yield return new WaitForSeconds(0.4f);
        gallery.SetActive(true);
        StartCoroutine(CreateGalleryWithNames());
        boardGallery.localScale = Vector3.one;
        buttonBackGallery.anchoredPosition = new Vector3(-112, buttonBack.anchoredPosition.y, 0);
        buttonTextGallery.anchoredPosition = new Vector3(buttonText.anchoredPosition.x, 160, 0);
        buttonBackGallery.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        buttonTextGallery.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
    }
    public void LeaderBoard()
    {
        if (PlayerPrefs.GetString("name") == "")
        {
            AudioManager.Play("click");
            popupName.gameObject.SetActive(true);
            popupName.idProfile = 2;
        }
        else
        {
            FirebaseManager.Instance.LogEvent("LEADERBOARD_ACCESS");
            StartCoroutine(EffectLeaderBoard());
        }
    }
    IEnumerator EffectLeaderBoard()
    {
        AudioManager.Play("click");
        buttonProfile.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        textLevel.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        btnMode.interactable = false;
        yield return new WaitForSeconds(0.4f);
        leaderBoard.SetActive(true);
        gameObject.SetActive(false);
    }
    public void Modelist()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_CHALLENGE_ACCESS");
        StartCoroutine(EffectMode());
    }
    IEnumerator EffectMode()
    {
        AudioManager.Play("click");
        buttonProfile.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(150, 0.4f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(-130, 0.4f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(130, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        textLevel.transform.DOScale(0, 0.4f).SetEase(Ease.OutQuart);
        buttonPlay.interactable = false;
        btnMode.interactable = false;
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
        loading.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        if (unlockedLevelsNumber == 1)
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
            GameObject loadedPrefab = Resources.Load<GameObject>((unlockedLevelsNumber - 1).ToString());
            GameObject level = Instantiate(loadedPrefab, main.transform);
            level.transform.SetParent(main.transform, false);
        }
        mainController.numberPlaying = unlockedLevelsNumber - 1;
        main.SetActive(true);
        gameObject.SetActive(false);
    }
    IEnumerator StartHome()
    {
        buttonPlay.DOKill();
        butTween1.Kill();
        buttonPlay.transform.localScale = Vector3.zero;
        textLevel.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.45f);
        buttonPlay.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        textLevel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.75f);
        butTween1 = buttonPlay.transform.DOScale(Vector3.one * 0.95f, 0.65f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
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
        buttonProfile.anchoredPosition = new Vector3(buttonProfile.anchoredPosition.x, 150, 0);
        buttonSetting.anchoredPosition = new Vector3(buttonSetting.anchoredPosition.x, 150, 0);
        buttonNoAds.anchoredPosition = new Vector3(buttonNoAds.anchoredPosition.x, 150, 0);
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonMode.anchoredPosition = new Vector3(-130, buttonMode.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonLeaderboard.anchoredPosition = new Vector3(130, buttonLeaderboard.anchoredPosition.y, 0);
        buttonProfile.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        textLevel.transform.localScale = new Vector3(0, 0, 1);
        buttonPlay.interactable = true;
        if (unlockedLevelsNumber > 10)
        {
            btnMode.interactable = true;
            textPlayMode.text = "CHALLENGE";
            iconLockMode.SetActive(false);
            iconUnlockMode.SetActive(true);
        }
        else
        {
            btnMode.interactable = false;
            textPlayMode.text = "UNLOCK LEVEL 10";
            iconLockMode.SetActive(true);
            iconUnlockMode.SetActive(false);
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
        buttonProfile.anchoredPosition = new Vector3(buttonProfile.anchoredPosition.x, 150, 0);
        buttonSetting.anchoredPosition = new Vector3(buttonSetting.anchoredPosition.x, 150, 0);
        buttonNoAds.anchoredPosition = new Vector3(buttonNoAds.anchoredPosition.x, 150, 0);
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonMode.anchoredPosition = new Vector3(-130, buttonMode.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonLeaderboard.anchoredPosition = new Vector3(130, buttonLeaderboard.anchoredPosition.y, 0);
        buttonProfile.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        textLevel.transform.localScale = new Vector3(0, 0, 1);
        buttonPlay.interactable = true;
        if (unlockedLevelsNumber > 10)
        {
            btnMode.interactable = true;
            textPlayMode.text = "CHALLENGE";
            iconLockMode.SetActive(false);
            iconUnlockMode.SetActive(true);
        }
        else
        {
            btnMode.interactable = false;
            textPlayMode.text = "UNLOCK LEVEL 10";
            iconLockMode.SetActive(true);
            iconUnlockMode.SetActive(false);
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
        buttonProfile.anchoredPosition = new Vector3(buttonProfile.anchoredPosition.x, 150, 0);
        buttonSetting.anchoredPosition = new Vector3(buttonSetting.anchoredPosition.x, 150, 0);
        buttonNoAds.anchoredPosition = new Vector3(buttonNoAds.anchoredPosition.x, 150, 0);
        buttonLevelList.anchoredPosition = new Vector3(-130, buttonLevelList.anchoredPosition.y, 0);
        buttonMode.anchoredPosition = new Vector3(-130, buttonMode.anchoredPosition.y, 0);
        buttonGallery.anchoredPosition = new Vector3(130, buttonGallery.anchoredPosition.y, 0);
        buttonLeaderboard.anchoredPosition = new Vector3(130, buttonLeaderboard.anchoredPosition.y, 0);
        buttonProfile.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonSetting.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonNoAds.DOAnchorPosY(-150, 0.5f).SetEase(Ease.OutQuart);
        buttonLevelList.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonMode.DOAnchorPosX(130, 0.5f).SetEase(Ease.OutQuart);
        buttonGallery.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonLeaderboard.DOAnchorPosX(-130, 0.5f).SetEase(Ease.OutQuart);
        buttonPlay.transform.localScale = new Vector3(0, 0, 1);
        textLevel.transform.localScale = new Vector3(0, 0, 1);
        buttonPlay.interactable = true;
        if (unlockedLevelsNumber > 10)
        {
            btnMode.interactable = true;
            textPlayMode.text = "CHALLENGE";
            iconLockMode.SetActive(false);
            iconUnlockMode.SetActive(true);
        }
        else
        {
            btnMode.interactable = false;
            textPlayMode.text = "UNLOCK LEVEL 10";
            iconLockMode.SetActive(true);
            iconUnlockMode.SetActive(false);
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
