using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameLevel3 : MonoBehaviour
{
    [SerializeField]
    private GamePlayLv3[] player;
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
    private Vector3 oldScale = new Vector3(100, 100, 1);
    // Start is called before the first frame update
    void Start()
    {
        if (hand != null)
        {
            oldHand = hand.position.x;
            hand.position = new Vector3(16, hand.position.y, hand.position.z);
        }
        main = GameManager.Instance.mainController;
        endGame = GameManager.Instance.endGame;
        losePanel = GameManager.Instance.losePanel;
        screenshotCamera = GameManager.Instance.mainCamera;
        rawImage = GameManager.Instance.screenShot;
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        takeShot = 0;
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        win = false;
        if (hand != null)
        {
            hand.DOMoveX(oldHand, 1f).SetEase(Ease.OutQuart);
        }
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
            for (int i = 0; i < player.Length; i++)
            {
                if (!player[i].locked)
                {
                    int index = i;
                    player[i].tideObject.SetActive(false);
                    player[i].playerRenderer.sprite = player[i].playerSprites[player[i].numberWin];
                    player[i].transform.DOMove(player[i].rightPos.position, 1.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        player[index].transform.DOMove(new Vector3(player[index].oldPosition.x, player[index].oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);

                    });
                    if (player[i].handTap != null)
                    {
                        player[i].handTap.SetActive(false);
                    }
                }
            }
            main.isHint = false;
        }
        if(transform.localScale != oldScale)
        {
            for (int i = 0; i < player.Length; i++)
            {
                player[i].boxCollider.size = Vector2.zero;
            }
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
