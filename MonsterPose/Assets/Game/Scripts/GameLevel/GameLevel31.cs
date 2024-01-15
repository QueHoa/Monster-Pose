using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameLevel31 : MonoBehaviour
{
    [SerializeField]
    private GamePlayLv2[] player;
    [SerializeField]
    private GamePlayLv31[] player2;
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
    private bool hint;
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
        hint = false;
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
        if (player.Length == 0)
        {
            win = true;
        }
        for (int i = 0; i < player2.Length; i++)
        {
            if (!player2[i].locked)
            {
                win2 = false;
                break;
            }
            if (i == player2.Length - 1 && player2[i].locked)
            {
                win2 = true;
            }
        }
        if (main.isHint && !hint)
        {
            hint = true;
            for (int i = 0; i < player2.Length; i++)
            {
                if (!player2[i].locked)
                {
                    int index = i;
                    player2[index].numHeart = player2[index].numberWin;
                    player2[index].playerRenderer.sprite = player2[index].playerSprites[player2[index].numberWin];
                    player2[index].transform.DOMove(player2[index].rightPos.position, 1.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        player2[index].transform.DOMove(new Vector3(player2[index].oldPosition.x, player2[index].oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            hint = false;
                            main.isHint = false;
                        });

                    });
                }
            }
            for (int i = 0; i < player.Length; i++)
            {
                if (!player[i].locked)
                {
                    int index = i;
                    player[index].playerRenderer.sprite = player[index].playerSprites[player[index].numberWin];
                    player[index].transform.DOMove(player[index].rightPos.position, 1.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        player[index].transform.DOMove(new Vector3(player[index].oldPosition.x, player[index].oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);

                    });
                }
            }
        }
        if (win && win2 && takeShot == 0)
        {
            StartCoroutine(Win());
            takeShot++;
        }
        for (int i = 0; i < player.Length; i++)
        {
            if (!player[i].isHand)
            {
                for (int j = 0; j < player.Length; j++)
                {
                    player[j].isHand = false;
                }
                for (int j = 0; j < player2.Length; j++)
                {
                    player2[j].isHand = false;
                }
                break;
            }
        }
        for (int i = 0; i < player2.Length; i++)
        {
            if (!player2[i].isHand)
            {
                for (int j = 0; j < player2.Length; j++)
                {
                    player2[j].isHand = false;
                }
                for (int j = 0; j < player.Length; j++)
                {
                    player[j].isHand = false;
                }
                break;
            }
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
