using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using OneHit.Framework;
using UnityEngine.EventSystems;
using TMPro;

public class GamePlayTutorial3 : MonoBehaviour
{
    public Transform rightPos;
    public GameObject hand;
    public GameObject hand2;
    public GameObject frame;
    public GameObject frame2;
    public GameObject fade;
    public SpriteRenderer playerRenderer;
    public Animator fail;

    public int numberWin;
    public Transform endHand;
    public Sprite[] playerSprites;

    [HideInInspector]
    public bool locked;

    private MainController main;
    private Camera mainCamera;    
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private GameObject losePanel;
    [HideInInspector]
    public int numHeart = 0;
    private int isTouch = 0;
    private float deltaX, deltaY;
    private bool isDrag;
    private bool isBeginInBox;
    [HideInInspector]
    public Vector2 oldPosition;
    [HideInInspector]
    public bool block;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Began());
        playerRenderer.sprite = playerSprites[0];
        losePanel = GameManager.Instance.losePanel;
        mainCamera = Camera.main;
        oldPosition = transform.position;
        transform.position = new Vector3(oldPosition.x - 6, oldPosition.y, 0);
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        main = GameManager.Instance.mainController;
        anim = GetComponent<Animator>();
        isDrag = false;
        locked = false;
        block = false;
        transform.DOMoveX(oldPosition.x, 1f).SetEase(Ease.OutQuart);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && !losePanel.activeInHierarchy && block)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPos = mainCamera.ScreenToWorldPoint(touch.position);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return;
                }
                if (touch.phase == TouchPhase.Began)
                {
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;
                    isTouch = 1;
                }
                if (touch.phase == TouchPhase.Moved && isTouch == 1)
                {
                    isDrag = true;
                    rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                }
                if (touch.phase == TouchPhase.Ended && isTouch == 1)
                {
                    if (Mathf.Abs(touchPos.x - deltaX - oldPosition.x) <= 0.15f && Mathf.Abs(touchPos.y - deltaY - oldPosition.y) <= 0.15f)
                    {
                        anim.SetTrigger("change");
                    }
                    else
                    {
                        if (Mathf.Abs(transform.position.x - rightPos.position.x) <= 0.8f && Mathf.Abs(transform.position.y - rightPos.position.y) <= 0.8f && playerRenderer.sprite == playerSprites[numberWin])
                        {
                            AudioManager.Play("hint");
                            anim.SetTrigger("complete");
                            transform.position = new Vector2(rightPos.position.x, rightPos.position.y);
                            locked = true;
                        }
                        else
                        {
                            AudioManager.Play("pen_fail");
                            anim.SetTrigger("fail");
                            fail.SetTrigger("show");
                            transform.DOMove(new Vector3(oldPosition.x, oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);
                        }
                    }
                    isTouch = 0;
                    isDrag = false;
                }
            }
        }
        anim.SetBool("drag", isDrag);
        if (locked)
        {
            hand2.SetActive(false);
            frame2.SetActive(false);
            fade.SetActive(false);
            losePanel.SetActive(false);
        }
        if (main.losePanel.gameObject.activeInHierarchy)
        {
            hand.SetActive(false);
            frame.SetActive(false);
            hand2.SetActive(false);
            frame2.SetActive(false);
        }
    }    
    bool IsWithinBoxCollider()
    {
        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);
        return boxCollider.OverlapPoint(touchPosition2D);
    }
    IEnumerator Began()
    {
        yield return new WaitForSeconds(1);
        fade.SetActive(true);
        hand.SetActive(true);
        frame.SetActive(true);

    }
    void ChangeSprite()
    {
        AudioManager.Play("change_sprite");
        if (numHeart != playerSprites.Length - 1)
        {
            numHeart++;
        }
        else
        {
            numHeart = 0;
        }
        playerRenderer.sprite = playerSprites[numHeart];
        if (playerRenderer.sprite == playerSprites[2])
        {
            hand2.SetActive(true);
            frame2.SetActive(true);
            //MoveTap();
        }
        else
        {
            hand2.SetActive(false);
            frame2.SetActive(false);
        }
    } 
    public void MoveTap()
    {
        hand2.transform.DOMove(endHand.position + new Vector3(0.5f, 4, 0), 1f)
            .OnComplete(() =>
            {
                hand2.transform.position = transform.position + new Vector3(0.5f, 4, 0);
                MoveTap();
            });
    }
}
