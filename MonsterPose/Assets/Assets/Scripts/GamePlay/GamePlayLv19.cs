using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using OneHit.Framework;

public class GamePlayLv19 : MonoBehaviour
{
    public GameObject handTap;
    [SerializeField]
    private Animator failTap;
    public Animator trueTap;
    public SpriteRenderer playerRenderer;

    public int numberWin;
    public Sprite[] playerSprites;

    [HideInInspector]
    public bool locked;

    private Animator anim;
    private MainController main;
    private int numHeart = 0;
    private float timeChange;

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer.sprite = playerSprites[0];
        anim = GetComponent<Animator>();
        main = GameManager.Instance.mainController;
        locked = false;
        timeChange = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
            if(touch.phase == TouchPhase.Began)
            {
                if(numHeart != numberWin)
                {
                    failTap.SetTrigger("show");
                }
                else
                {
                    trueTap.SetTrigger("show");
                    AudioManager.Play("hint");
                    if (handTap != null)
                    {
                        handTap.SetActive(false);
                    }
                    locked = true;
                }
            }
        }
        timeChange -= Time.deltaTime;
        if (timeChange < 0 && !locked && !main.isHint && !main.isWin)
        {
            anim.SetTrigger("change");
            timeChange = 2;
        }
    }
    void ChangeSprite()
    {
        if(numHeart != playerSprites.Length - 1)
        {
            numHeart++;
        }
        else
        {
            numHeart = 0;
        }
        playerRenderer.sprite = playerSprites[numHeart];
    }
}
