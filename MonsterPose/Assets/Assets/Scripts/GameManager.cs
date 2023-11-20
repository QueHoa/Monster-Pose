using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;
using OneHit;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Start Show Ad")]
    public static float levelShowAd = 3f;
    [Header("Number Level Classic")]
    public static int numberLevel;
    [Header("Number Level Mode")]
    public static int numberMode;
    [Header("Test Monster Tutorial")]
    public static float numberMonster = Random.Range(1,3);

    public GameObject main;
    public GameObject mode;
    public GameObject home;
    public GameObject endGame;
    public GameObject endGameMode;
    public GameObject losePanel;
    public GameObject levelList;
    public GameObject levelListMode;
    public GameObject gallery;
    public GameObject photo;
    public GameObject loading;
    public Transform frameImageWin;
    public Image imageWin;
    public Text textHeart;
    public Text levelPhoto;
    public MainController mainController;
    public ModeController modeController;
    public HomeController homeController;
    public Camera mainCamera;
    public RawImage screenShot;
    [SerializeField]
    private Image onVibrate;
    [SerializeField]
    private Image onVibrate2;
    [SerializeField]
    private Image offVibrate;
    [SerializeField]
    private Image onMusic;
    [SerializeField]
    private Image onMusic2;
    [SerializeField]
    private Image offMusic;
    [SerializeField]
    private Image onSound;
    [SerializeField]
    private Image onSound2;
    [SerializeField]
    private Image offSound;
    [HideInInspector]
    public int isVibrate;
    [HideInInspector]
    public int isMusic;
    [HideInInspector]
    public int isSound;

    private int unlockedLevelsNumber;
    private int unlockedModesNumber;
    // Start is called before the first frame update
    void Start()
    {
        if (isMusic == 1)
        {
            AudioManager.PlayBGM();
        }
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
        Instance = this;
        //PlayerPrefs.SetInt("levelsUnlocked", 1);
        if (!PlayerPrefs.HasKey("levelsUnlocked"))
        {
            PlayerPrefs.SetInt("levelsUnlocked", 1);
        }
        //PlayerPrefs.SetInt("levelsModeUnlocked", 1);
        if (!PlayerPrefs.HasKey("levelsModeUnlocked"))
        {
            PlayerPrefs.SetInt("levelsModeUnlocked", 1);
        }
        if (!PlayerPrefs.HasKey("MusicOn"))
        {
            PlayerPrefs.SetInt("MusicOn", 1);
        }
        if (!PlayerPrefs.HasKey("SoundOn"))
        {
            PlayerPrefs.SetInt("SoundOn", 1);
        }
        if (!PlayerPrefs.HasKey("VibrateOn"))
        {
            PlayerPrefs.SetInt("VibrateOn", 1);
        }
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        numberLevel = homeController.numberLevel;
        numberMode = homeController.numberMode;
        MasterControl.Instance.ShowBanner();
        
        isMusic = PlayerPrefs.GetInt("MusicOn");
        isSound = PlayerPrefs.GetInt("SoundOn");
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        if (isMusic == 1)
        {
            onMusic.enabled = true;
            onMusic2.enabled = true;
            offMusic.enabled = false;
        }
        else
        {
            onMusic.enabled = false;
            onMusic2.enabled = false;
            offMusic.enabled = true;
        }
        if (isSound == 1)
        {
            onSound.enabled = true;
            onSound2.enabled = true;
            offSound.enabled = false;
        }
        else
        {
            onSound.enabled = false;
            onSound2.enabled = false;
            offSound.enabled = true;
        }
        if (isVibrate == 1)
        {
            onVibrate.enabled = true;
            onVibrate2.enabled = true;
            offVibrate.enabled = false;
        }
        else
        {
            onVibrate.enabled = false;
            onVibrate2.enabled = false;
            offVibrate.enabled = true;
        }
    }   

    // Update is called once per frame
    void Update()
    {
        isMusic = PlayerPrefs.GetInt("MusicOn");
        isSound = PlayerPrefs.GetInt("SoundOn");
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
    }
    public void SetMusic()
    {
        AudioManager.UpdateMusic();
        AudioManager.Play("click");
        if (isMusic == 1)
        {
            onMusic.enabled = false;
            onMusic2.enabled = false;
            offMusic.enabled = true;
            PlayerPrefs.SetInt("MusicOn", 0);
        }
        else
        {
            onMusic.enabled = true;
            onMusic2.enabled = true;
            offMusic.enabled = false;
            PlayerPrefs.SetInt("MusicOn", 1);
        }
    }
    public void SetVibrate()
    {
        AudioManager.Play("click");
        if (isVibrate == 1)
        {
            onVibrate.enabled = false;
            onVibrate2.enabled = false;
            offVibrate.enabled = true;
            PlayerPrefs.SetInt("VibrateOn", 0);
        }
        else
        {
            onVibrate.enabled = true;
            onVibrate2.enabled = true;
            offVibrate.enabled = false;
            PlayerPrefs.SetInt("VibrateOn", 1);
        }
    }
    public void SetSound()
    {
        if (isSound == 1)
        {
            onSound.enabled = false;
            onSound2.enabled = false;
            offSound.enabled = true;
            PlayerPrefs.SetInt("SoundOn", 0);
        }
        else
        {
            onSound.enabled = true;
            onSound2.enabled = true;
            offSound.enabled = false;
            PlayerPrefs.SetInt("SoundOn", 1);
        }
        AudioManager.Play("click");
    }
}
