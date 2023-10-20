using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameLevel19 : MonoBehaviour
{
    [SerializeField]
    private GamePlayLv19 player;
    [SerializeField]
    private GameObject heart;
    private MainController main;
    private GameObject endGame;
    private GameObject losePanel;
    public HapticTypes hapticTypes = HapticTypes.Failure;
    private bool hapticsAllowed = true;
    private bool win;
    private int takeShot;
    private Camera screenshotCamera;
    private RawImage rawImage;
    private int isVibrate;
    // Start is called before the first frame update
    void Start()
    {
        main = GameManager.Instance.mainController;
        endGame = GameManager.Instance.endGame;
        losePanel = GameManager.Instance.losePanel;
        screenshotCamera = GameManager.Instance.mainCamera;
        rawImage = GameManager.Instance.screenShot;
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        takeShot = 0;
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        win = false;
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
            player.playerRenderer.sprite = player.playerSprites[player.numberWin];
            player.handTap.SetActive(false);
            player.trueTap.SetTrigger("show");
            win = true;
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
        yield return new WaitForSeconds(0.3f);
        main.isHint = false;
        if (isVibrate == 1)
        {
            MMVibrationManager.Haptic(hapticTypes, true, true, this);
        }
        heart.SetActive(false);
        losePanel.SetActive(false);
        ScreenshotWin.Screenshot(screenshotCamera, rawImage);
        yield return new WaitForSeconds(0.15f);
        endGame.SetActive(true);
    }
}
