using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using OneHit.Framework;
using UnityEngine.EventSystems;
using TMPro;

public class GamePlayTutorial2 : MonoBehaviour
{
    public Transform rightPos;
    public GameObject handTapAnim;
    public GameObject handTap;
    public GameObject frame;
    public GameObject frame2;
    public GameObject frame3;
    public GameObject fade;
    public SpriteRenderer playerRenderer;
    public SpriteRenderer heart;
    public SpriteRenderer hand;

    public int numberWin;
    public Transform endHand;
    public Sprite[] playerSprites;

    [HideInInspector]
    public bool locked;   

    private Camera mainCamera;    
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private GameObject losePanel;
    private int numHeart = 0;   
    private float deltaX, deltaY;
    private bool isDrag;
    private bool isTutorial;
    private bool isBeginInBox;
    private Vector2 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        isTutorial = true;
        StartCoroutine(Tutorial());
        playerRenderer.sprite = playerSprites[0];
        losePanel = GameManager.Instance.losePanel;
        mainCamera = Camera.main;
        oldPosition = transform.position;
        transform.position = new Vector3(oldPosition.x - 6, oldPosition.y, 0);
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isDrag = false;
        locked = false;
        fade.SetActive(false);
        transform.DOMoveX(oldPosition.x, 1f).SetEase(Ease.OutQuart);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && !isTutorial && !losePanel.activeInHierarchy)
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
                    if (IsWithinBoxCollider())
                    {
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;
                        isBeginInBox = true;
                    }
                    else
                    {
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;
                        isBeginInBox = false;
                    }
                }                  
                if (playerRenderer.sprite == playerSprites[2] && touch.phase == TouchPhase.Moved && IsWithinBoxCollider())
                {
                    isDrag = true;
                    rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                }
                if (playerRenderer.sprite != playerSprites[2] && touch.phase == TouchPhase.Moved && IsWithinBoxCollider())
                {
                    transform.position = new Vector2(oldPosition.x, oldPosition.y);
                }
                if (playerRenderer.sprite != playerSprites[2] && touch.phase == TouchPhase.Ended)
                {
                    if (Mathf.Abs(touchPos.x - deltaX - oldPosition.x) <= 0.15f && Mathf.Abs(touchPos.y - deltaY - oldPosition.y) <= 0.15f && IsWithinBoxCollider())
                    {
                        anim.SetTrigger("change");
                    }
                    if ((Mathf.Abs(touchPos.x - deltaX - oldPosition.x) > 0.15f || Mathf.Abs(touchPos.y - deltaY - oldPosition.y) > 0.15f) && isBeginInBox)
                    {
                        AudioManager.Play("pen_fail");
                        anim.SetTrigger("fail");
                    }  
                }                
                if (playerRenderer.sprite == playerSprites[2] && touch.phase == TouchPhase.Ended && IsWithinBoxCollider())
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
                        transform.DOMove(new Vector3(oldPosition.x, oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);
                    }
                    isDrag = false;
                }                
            }
        }
        anim.SetBool("drag", isDrag);
        if (locked)
        {
            handTap.SetActive(false);
            frame3.SetActive(false);
            fade.SetActive(false);
            losePanel.SetActive(false);
        }
    }    
    bool IsWithinBoxCollider()
    {
        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);
        return boxCollider.OverlapPoint(touchPosition2D);
    }
    void ChangeSprite()
    {
        AudioManager.Play("change_sprite");
        if (numHeart != playerSprites.Length - 1)
        {
            numHeart++;
        }           
        playerRenderer.sprite = playerSprites[numHeart];
        if (playerRenderer.sprite == playerSprites[1])
        {
            frame.SetActive(false);
            frame2.SetActive(true);
        }
        if (playerRenderer.sprite == playerSprites[2])
        {
            handTapAnim.SetActive(false);
            frame2.SetActive(false);
            frame3.SetActive(true);
            handTap.SetActive(true);
            heart.sortingOrder = 4;
            hand.sortingOrder = 6;
            MoveTap();
        }
    }    
    IEnumerator Tutorial()
    {
        Input.multiTouchEnabled = false;
        yield return new WaitForSeconds(1f);
        Input.multiTouchEnabled = true;
        isTutorial = false;
        fade.SetActive(true);
        handTapAnim.SetActive(true);
        frame.SetActive(true);
    }
    private void MoveTap()
    {
        handTap.transform.DOMove(endHand.position + new Vector3(0, 4, 0), 1f)
            .OnComplete(() =>
            {
                handTap.transform.position = transform.position + new Vector3(0, 4, 0);
                MoveTap();
            });
    }
}
