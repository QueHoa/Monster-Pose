using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using OneHit.Framework;
using UnityEngine.EventSystems;

public class GamePlayLv18 : MonoBehaviour
{
    public Transform rightPos;
    public GameObject tideObject;
    public ParticleSystem effect;
    public SpriteRenderer playerRenderer;
    public Animator fail;

    public int numberWin;
    public Sprite[] playerSprites;

    [HideInInspector]
    public bool locked;

    private Camera mainCamera;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private GameObject losePanel;
    private MainController main;
    private int numHeart = 0;
    private int isTouch = 0;
    private float deltaX, deltaY;
    private bool isDrag;
    [HideInInspector]
    public Vector2 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        oldPosition = transform.position;
        transform.position = new Vector3(oldPosition.x - 6, oldPosition.y, 0);
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        losePanel = GameManager.Instance.losePanel;
        main = GameManager.Instance.mainController;
        isDrag = false;
        locked = false;
        transform.DOMoveX(oldPosition.x, 1f).SetEase(Ease.OutQuart);
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && IsWithinBoxCollider() && !losePanel.activeInHierarchy && !main.isHint)
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
                    isDrag = false;
                    isTouch = 0;
                }
            }
        }
        anim.SetBool("drag", isDrag);
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
        numHeart++;
        playerRenderer.sprite = playerSprites[numHeart];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tideObject.GetComponent<SideGamePlay3>().numHeart >0)
        {
            ChangeSprite();
            effect.Play();
            tideObject.SetActive(false);
        }
    }
}
