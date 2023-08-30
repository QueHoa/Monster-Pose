using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameLevel18 : MonoBehaviour
{
    [SerializeField]
    private GamePlayLv18[] player;
    [SerializeField]
    private GameObject heart;
    [SerializeField]
    private Transform hand;
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
    private float oldHand;
    // Start is called before the first frame update
    void Start()
    {
        oldHand = hand.position.x;
        hand.position = new Vector3(16, hand.position.y, hand.position.z);
        main = GameManager.Instance.mainController;
        endGame = GameManager.Instance.endGame;
        losePanel = GameManager.Instance.losePanel;
        screenshotCamera = GameManager.Instance.mainCamera;
        rawImage = GameManager.Instance.screenShot;
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        takeShot = 0;
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
        win = false;
        hand.DOMoveX(oldHand, 0.8f).SetEase(Ease.OutQuart);
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
                player[i].effect.Play();
                player[i].tideObject.SetActive(false);
                player[i].playerRenderer.sprite = player[i].playerSprites[player[i].numberWin];
                player[i].transform.DOMove(player[i].rightPos.position, 1.2f).SetEase(Ease.OutQuart);
            }
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
