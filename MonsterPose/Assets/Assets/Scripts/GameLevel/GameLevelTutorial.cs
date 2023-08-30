using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MoreMountains.NiceVibrations;

public class GameLevelTutorial : MonoBehaviour
{
    [SerializeField]
    private GamePlayTutorial[] player;
    [SerializeField]
    private Sprite img3;
    [SerializeField]
    private GameObject heart;
    [SerializeField]
    private GameObject handTap;
    private GameObject endGame;
    private MainController main;
    public HapticTypes hapticTypes = HapticTypes.HeavyImpact;
    private bool hapticsAllowed = true;
    private int takeShot;
    private bool win;
    private Animator anim;
    private Camera screenshotCamera; // Tham chiếu đến Camera bạn muốn sử dụng để chụp màn hình
    private RawImage rawImage; // Tham chiếu đến RawImage để hiển thị ảnh chụp màn hình
    private int isVibrate;

    // Start is called before the first frame update
    void Start()
    {
        endGame = GameManager.Instance.endGame;
        main = GameManager.Instance.mainController;
        screenshotCamera = GameManager.Instance.mainCamera;
        rawImage = GameManager.Instance.screenShot;
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        anim = GetComponent<Animator>();
        StartCoroutine(Tutorial());
        win = false;
        takeShot = 0;
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < player.Length; i++)
        {
            if (!player[i].locked)
            {
                win = false;
                break;
            }
            if (i == player.Length - 1 && player[i].locked)
            {
                win = true;
            }
        }
        if (main.isHint)
        {
            main.isHint = false;
        }
        if (win && takeShot == 0)
        {
            StartCoroutine(Win());
            takeShot++;
        }
    }
    IEnumerator Tutorial()
    {
        anim.SetTrigger("tutorial");
        Input.multiTouchEnabled = false;
        yield return new WaitForSeconds(2.3f);
        Input.multiTouchEnabled = true;
    }
    private void ShowHandTap()
    {
        handTap.SetActive(true);
    }
    IEnumerator Win()
    {
        main.isWin = true;
        yield return new WaitForSeconds(0.2f);
        if (isVibrate == 1)
        {
            MMVibrationManager.Haptic(hapticTypes, true, true, this);
        }
        heart.SetActive(false);
        ScreenshotWin.Screenshot(screenshotCamera, rawImage);
        endGame.SetActive(true);
    }
}
