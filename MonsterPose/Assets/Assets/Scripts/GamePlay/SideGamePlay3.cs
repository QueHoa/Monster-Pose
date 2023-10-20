using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class SideGamePlay3 : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerRenderer;

    [HideInInspector]
    public int numHeart = 0;
    public Sprite[] playerSprites;

    private Camera mainCamera;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private GameObject losePanel;
    private int isTouch = 0;
    private float deltaX, deltaY;
    private bool isDrag;
    [HideInInspector]
    public Vector2 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerRenderer.sprite = playerSprites[0];
        mainCamera = Camera.main;
        oldPosition = transform.position;
        transform.position = new Vector3(oldPosition.x + 6, oldPosition.y, 0);
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        losePanel = GameManager.Instance.losePanel;
        isDrag = false;
        transform.DOMoveX(oldPosition.x, 0.8f).SetEase(Ease.OutQuart);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWithinBoxCollider() && !losePanel.activeInHierarchy)
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
                        anim.SetTrigger("fail");
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
        numHeart++;
        playerRenderer.sprite = playerSprites[numHeart];
    }
}
