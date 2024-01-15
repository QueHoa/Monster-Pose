using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using OneHit.Framework;

public class ChoosePhoto : MonoBehaviour
{
    [SerializeField]
    private Text numberPhoto;
    [SerializeField]
    private Image BG;
    [SerializeField]
    private GameObject lockPhoto;

    private GameObject home;
    private GameObject photo;
    private Transform frameImageWin;
    private Image imageWin;
    private Text levelPhoto;
    private Text textHeart;
    private int unlockedLevelsNumber;
    private int value;
    private int numberHeart;
    void Start()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        transform.localScale = new Vector3(0, 0, 1);
        home = GameManager.Instance.home;
        photo = GameManager.Instance.photo;
        frameImageWin = GameManager.Instance.frameImageWin;
        imageWin = GameManager.Instance.imageWin;
        levelPhoto = GameManager.Instance.levelPhoto;
        textHeart = GameManager.Instance.textHeart;
        value = int.Parse(gameObject.name);
        numberHeart = PlayerPrefs.GetInt(value.ToString());
        value += 1;
        numberPhoto.text = "Photo " + value.ToString();
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuart);
        if (value < unlockedLevelsNumber)
        {
            gameObject.GetComponent<Button>().interactable = true;
            lockPhoto.SetActive(false);
            string filePath = Application.persistentDataPath + "/" + value.ToString() + ".jpg"; ;
            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                Texture2D loadedTexture = new Texture2D(2, 2);
                loadedTexture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));
                BG.sprite = sprite;
            }
        }
        else
        {
            lockPhoto.SetActive(true);
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    void Update()
    {
        
    }

    public void Choose()
    {
        StartCoroutine(EffectChoose());
    }
    IEnumerator EffectChoose()
    {
        AudioManager.Play("click");
        imageWin.sprite = BG.sprite;
        textHeart.text = "LOVE: " + numberHeart.ToString();
        frameImageWin.localScale = Vector3.zero;
        frameImageWin.position = transform.position;
        frameImageWin.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuart);
        frameImageWin.DOMove(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);
        levelPhoto.text = "Photo " + value.ToString();
        /*homeController.buttonBackGallery.DOAnchorPosX(-112, 0.5f).SetEase(Ease.OutQuart);
        homeController.buttonTextGallery.DOAnchorPosY(160, 0.5f).SetEase(Ease.OutQuart);
        homeController.boardGallery.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);*/
        photo.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }
}
