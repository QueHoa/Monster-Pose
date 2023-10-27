using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MoreMountains.NiceVibrations;
using DG.Tweening;

public class GameLevelTutorial2 : MonoBehaviour
{
    public GamePlayTutorial2 player;
    public GameObject heart;
    public Transform hand;
    private GameObject endGame;
    private MainController main;
    public HapticTypes hapticTypes = HapticTypes.HeavyImpact;
    private bool hapticsAllowed = true;
    private int takeShot;
    private bool win;
    private Camera screenshotCamera; // Tham chiếu đến Camera bạn muốn sử dụng để chụp màn hình
    private RawImage rawImage; // Tham chiếu đến RawImage để hiển thị ảnh chụp màn hình
    private int isVibrate;
    private float oldHand;

    // Start is called before the first frame update
    void Start()
    {
        oldHand = hand.position.x;
        hand.position = new Vector3(16, hand.position.y, hand.position.z);
        endGame = GameManager.Instance.endGame;
        main = GameManager.Instance.mainController;
        screenshotCamera = GameManager.Instance.mainCamera;
        rawImage = GameManager.Instance.screenShot;
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        win = false;
        takeShot = 0;
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        hand.DOMoveX(oldHand, 1f).SetEase(Ease.OutQuart);
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
        yield return new WaitForSeconds(0.15f);
        endGame.SetActive(true);
    }
}
