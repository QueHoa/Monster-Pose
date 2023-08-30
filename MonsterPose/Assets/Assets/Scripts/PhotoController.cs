using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PhotoController : MonoBehaviour
{
    public RectTransform back;
    public RectTransform title;
    public RectTransform image;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        back.anchoredPosition = new Vector3(-112, back.anchoredPosition.y, 0);
        title.anchoredPosition = new Vector3(title.anchoredPosition.x, 160, 0);
        image.localScale = Vector3.zero;
        back.DOAnchorPosX(112, 0.5f).SetEase(Ease.OutQuart);
        title.DOAnchorPosY(-160, 0.5f).SetEase(Ease.OutQuart);
        image.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuart);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
