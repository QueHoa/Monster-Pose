using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MoreMountains.NiceVibrations;
using DG.Tweening;

public class GameLevelTutorial3 : MonoBehaviour
{
    public GamePlayTutorial3[] player;
    public GameObject heart;
    public Transform hand;
    public HapticTypes hapticTypes = HapticTypes.Failure;
    private bool hapticsAllowed = true;
    private GameObject endGame;
    private GameObject losePanel;
    private MainController main;
    private int takeShot;
    private bool win;
    private Camera screenshotCamera;
    private RawImage rawImage;
    private int isVibrate;
    private float oldHand;
    // Start is called before the first frame update
    void Start()
    {
        if (hand != null)
        {
            oldHand = hand.position.x;
            hand.position = new Vector3(16, hand.position.y, hand.position.z);
        }
        endGame = GameManager.Instance.endGame;
        losePanel = GameManager.Instance.losePanel;
        screenshotCamera = GameManager.Instance.mainCamera;
        rawImage = GameManager.Instance.screenShot;
        main = GameManager.Instance.mainController;
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        win = false;
        takeShot = 0;
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        hand.DOMoveX(oldHand, 1f).SetEase(Ease.OutQuart);
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
            NewMethod();
        }
        if (win && takeShot == 0)
        {
            StartCoroutine(Win());
            takeShot++;
        }
    }
    [ContextMenu("btn")]
    private void NewMethod()
    {
        for (int i = 0; i < player.Length; i++)
        {
            if (!player[i].locked)
            {
                int index = i;
                player[index].playerRenderer.sprite = player[index].playerSprites[player[index].numberWin];
                player[index].numHeart = player[index].numberWin;
                player[index].block = true;
                player[index].fade.SetActive(false);
                player[index].frame.SetActive(false);
                player[index].hand.SetActive(false);
                player[index].transform.DOMove(player[index].rightPos.position, 1f).SetEase(Ease.OutQuart).OnComplete(() =>
                {
                    player[index].transform.DOMove(new Vector3(player[index].oldPosition.x, player[index].oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        player[index].hand2.SetActive(true);
                        player[index].frame2.SetActive(true);
                        player[index].MoveTap();
                    });
                });
            }
        }
        main.isHint = false;
    }

    IEnumerator Win()
    {
        main.isWin = true;
        yield return new WaitForSeconds(0.3f);
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
