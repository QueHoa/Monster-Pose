using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameLevel31 : MonoBehaviour
{
    [SerializeField]
    private GamePlayLv2 player;
    [SerializeField]
    private GamePlayLv31 player2;
    [SerializeField]
    private GameObject heart;
    private MainController main;
    private GameObject endGame;
    private GameObject losePanel;
    public HapticTypes hapticTypes = HapticTypes.Failure;
    private bool hapticsAllowed = true;
    private int takeShot;
    private Camera screenshotCamera;
    private RawImage rawImage;
    private int isVibrate;
    private bool win;
    private bool win2;
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
        win2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.locked)
        {
            win = true;
        }
        else
        {
            win = false;
        }
        if (player2.locked)
        {
            win2 = true;
        }
        else
        {
            win2 = false;
        }
        if (main.isHint)
        {
            player2.numHeart = player2.numberWin;
            player2.playerRenderer.sprite = player2.playerSprites[player2.numberWin];
            player2.transform.DOMove(player2.rightPos.position, 1.2f).SetEase(Ease.OutQuart);
            player.transform.DOMove(player.rightPos.position, 1.2f).SetEase(Ease.OutQuart);
            win = true;
            win2 = true;
        }
        if (win && win2 && takeShot == 0)
        {
            StartCoroutine(Win());
            takeShot++;
        }
    }
    IEnumerator Win()
    {
        main.isWin = true;
        if (main.isHint)
        {
            main.isHint = false;
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }
        if (isVibrate == 1)
        {
            MMVibrationManager.Haptic(hapticTypes, true, true, this);
        }
        heart.SetActive(false);
        losePanel.SetActive(false);
        ScreenshotWin.Screenshot(screenshotCamera, rawImage);
        endGame.SetActive(true);
    }
}
