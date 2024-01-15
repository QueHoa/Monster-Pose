using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MoreMountains.NiceVibrations;

public class GameLevelTutorial : MonoBehaviour
{
    [SerializeField]
    private GamePlayTutorial player;
    [SerializeField]
    private Sprite img3;
    [SerializeField]
    private GameObject heart;
    [SerializeField]
    private GameObject handTap;
    public GameObject frame2;
    public GameObject frame3;
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
        frame2.SetActive(false);
        frame3.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.locked)
        {
            win = false;
        }
        if (player.locked)
        {
            win = true;
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
        yield return new WaitForSeconds(7.5f);
        Input.multiTouchEnabled = true;
    }
    private void ShowHandTap()
    {
        handTap.SetActive(true);
        player.ChangeSprite();
    }
    IEnumerator Win()
    {
        main.isWin = true;
        frame2.SetActive(false);
        frame3.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        if (isVibrate == 1)
        {
            MMVibrationManager.Haptic(hapticTypes, true, true, this);
        }
        heart.SetActive(false);
        ScreenshotWin.Screenshot(screenshotCamera, rawImage);
        yield return new WaitForSeconds(0.15f);
        endGame.SetActive(true);
    }
    public void setFrame2()
    {
        frame2.SetActive(true);
        frame3.SetActive(true);
    }
}
