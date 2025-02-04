using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OneHit;
using OneHit.Framework;

public class PhotoController : MonoBehaviour
{
    public Text level;
    public RectTransform back;
    public RectTransform title;
    public RectTransform image;
    public CanvasGroup alpha;
    public Image imageWin;
    private int unlockedLevelsNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        alpha.alpha = 0;
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        back.anchoredPosition = new Vector3(-112, back.anchoredPosition.y, 0);
        title.anchoredPosition = new Vector3(title.anchoredPosition.x, 160, 0);
        //image.localScale = Vector3.zero;
        back.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        title.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
        //image.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuart);
    }
    // Update is called once per frame
    void Update()
    {
        DOTween.To(() => alpha.alpha, x => alpha.alpha = x, 1, 0.5f);
    }
    public void ShareImage()
    {
        AudioManager.Play("click");
        Texture2D text = (Texture2D)imageWin.sprite.texture;
        
        if (unlockedLevelsNumber > GameManager.levelShowAd)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                if (Application.isMobilePlatform)
                {
                    NativeShare nativeShare = new NativeShare();
                    nativeShare.AddFile(text, level.text + ".jpg");
                    nativeShare.Share();
                }
            });
        }
        else
        {
            if (Application.isMobilePlatform)
            {
                NativeShare nativeShare = new NativeShare();
                nativeShare.AddFile(text, level.text + ".jpg");
                nativeShare.Share();
            }
        }
    }
}
