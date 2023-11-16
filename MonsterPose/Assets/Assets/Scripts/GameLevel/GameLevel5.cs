using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameLevel5 : MonoBehaviour
{
    [SerializeField]
    private GamePlayLv2[] player;
    [SerializeField]
    private SideGamePlay2[] sidePlayer;
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
        for (int i = 0; i < sidePlayer.Length; i++)
        {
            if (!sidePlayer[i].locked)
            {
                win2 = false;
                break;
            }
            if (i == sidePlayer.Length - 1 && sidePlayer[i].locked)
            {
                win2 = true;
            }
        }
        if (main.isHint && !hint)
        {
            hint = true;
            for (int i = 0; i < player.Length; i++)
            {
                if (!player[i].locked)
                {
                    int index = i;
                    player[i].playerRenderer.sprite = player[i].playerSprites[player[i].numberWin];
                    player[i].transform.DOMove(player[i].rightPos.position, 1.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        player[index].transform.DOMove(new Vector3(player[index].oldPosition.x, player[index].oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            hint = false;
                            main.isHint = false;
                        });

                    });
                    if (player[i].handTap != null)
                    {
                        player[i].isHand = false;
                    }
                }
            }
            for (int i = 0; i < sidePlayer.Length; i++)
            {
                if (!sidePlayer[i].locked)
                {
                    int index = i;
                    sidePlayer[i].transform.DOMove(sidePlayer[i].rightPos.position, 1.2f).SetEase(Ease.OutQuart).OnComplete(() =>
                    {
                        sidePlayer[index].transform.DOMove(new Vector3(sidePlayer[index].oldPosition.x, sidePlayer[index].oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);

                    });
                }
                
            }
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
