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
    private GameObject gallery;
    private GameObject photo;
    private Image imageWin;
    private HomeController homeController;
    private int unlockedLevelsNumber;
    private int value;
    void Start()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        transform.localScale = new Vector3(0, 0, 1);
        home = GameManager.Instance.home;
        gallery = GameManager.Instance.gallery;
        photo = GameManager.Instance.photo;
        imageWin = GameManager.Instance.imageWin;
        homeController = GameManager.Instance.homeController;
        value = int.Parse(gameObject.name);
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
        homeController.buttonBackGallery.DOAnchorPosX(-112, 0.5f).SetEase(Ease.OutQuart);
        homeController.buttonTextGallery.DOAnchorPosY(160, 0.5f).SetEase(Ease.OutQuart);
        homeController.boardGallery.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        imageWin.sprite = BG.sprite;
        gallery.SetActive(false);
        photo.SetActive(true);
    }
}
