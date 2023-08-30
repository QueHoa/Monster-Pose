using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameLevel13 : MonoBehaviour
{
    [SerializeField]
    private GamePlayLv13 player;
    [SerializeField]
    private Transform bee1;
    [SerializeField]
    private Transform bee2;
    [SerializeField]
    private GameObject heart;
    [SerializeField]
    private Transform hand;
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
    private float oldHand;
    private Vector3 oldBee1 = Vector3.zero;
    private Vector3 oldBee2 = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        oldHand = hand.position.x;
        hand.position = new Vector3(16, hand.position.y, hand.position.z);
        oldBee1 = bee1.position;
        oldBee2 = bee2.position;
        main = GameManager.Instance.mainController;
        endGame = GameManager.Instance.endGame;
        losePanel = GameManager.Instance.losePanel;
        screenshotCamera = GameManager.Instance.mainCamera;
        rawImage = GameManager.Instance.screenShot;
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        takeShot = 0;
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        win = false;
        bee1.position = new Vector3(-10, oldBee1.y, oldBee1.z);
        bee2.position = new Vector3(-10, oldBee2.y, oldBee2.z);
        hand.DOMoveX(oldHand, 0.8f).SetEase(Ease.OutQuart);
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
        if (main.isHint)
        {
            player.playerRenderer.sprite = player.playerSprites[player.numberWin];
            player.transform.DOMove(player.rightPos.position, 1f).SetEase(Ease.OutQuart);
            
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
        StartCoroutine(BeeMove());
        if (main.isHint)
        {
            main.isHint = false;
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
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
    IEnumerator BeeMove()
    {
        bee1.DOMoveX(oldBee1.x, 0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.3f);
        bee2.DOMoveX(oldBee2.x, 0.5f).SetEase(Ease.OutCubic);
    }
}
