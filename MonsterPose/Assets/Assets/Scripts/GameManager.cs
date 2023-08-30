using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject loading;
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
    public Image imageWin;
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

    private int noLoading;
    private int unlockedLevelsNumber;
    private int unlockedModesNumber;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        noLoading = 0;
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
    private void Awake()
    {
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
        if (!PlayerPrefs.HasKey("gold"))
        {
            PlayerPrefs.SetInt("gold", 0);
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
        GameObject loadedPrefab = Resources.Load<GameObject>((unlockedLevelsNumber - 1).ToString());
        GameObject level = Instantiate(loadedPrefab, main.transform);
        level.transform.SetParent(main.transform, false);
    }   

    // Update is called once per frame
    void Update()
    {

        isMusic = PlayerPrefs.GetInt("MusicOn");
        isSound = PlayerPrefs.GetInt("SoundOn");
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        if (!loading.activeInHierarchy && noLoading == 0 && isMusic == 1)
        {
            AudioManager.PlayBGM();
            noLoading++;
        }
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
        AudioManager.Play("click");
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
    }
}
