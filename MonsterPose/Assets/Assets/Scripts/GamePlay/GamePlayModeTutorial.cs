
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using OneHit.Framework;
using UnityEngine.EventSystems;

public class GamePlayModeTutorial : MonoBehaviour
{
    public SpriteRenderer[] rightPos;
    public SpriteRenderer playerRenderer;
    public GameModeTutorial gameModeToturial;
    public int numberSprite;
    public bool locked;

    private Camera mainCamera;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;
    private Animator anim;
    private int isTouch = 0;
    private float deltaX, deltaY;
    private float time;
    private Vector3 oldPosition;
    private bool isDrag;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        oldPosition = transform.position;
        isDrag = false;
        locked = false;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("drag", isDrag);
        if (!gameModeToturial.isTutorial)
        {
            time += Time.deltaTime;
        }
        if (!locked && IsWithinBoxCollider() && !gameModeToturial.isTutorial)
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
                    boxCollider.size = new Vector2(10f, 15);
                }
                if (touch.phase == TouchPhase.Moved && isTouch == 1)
                {
                    isDrag = true;
                    rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                }
                if (touch.phase == TouchPhase.Ended && isTouch == 1)
                {
                    if (gameModeToturial.numbers[numberSprite] < rightPos.Length)
                    {
                        for (int i = 0; i < rightPos.Length; i++)
                        {
                            if (gameModeToturial.numbers[numberSprite] == i && rightPos[i].gameObject.activeInHierarchy)
                            {
                                if (-0.2f <= transform.position.y - rightPos[i].transform.position.y && transform.position.y - rightPos[i].transform.position.y <= 2f && Mathf.Abs(transform.position.x - rightPos[i].transform.position.x) <= 1f)
                                {
                                    AudioManager.Play("hint");
                                    anim.SetTrigger("complete");
                                    transform.position = new Vector3(rightPos[i].transform.position.x, rightPos[i].transform.position.y, 0);
                                    locked = true;
                                    gameModeToturial.numberLock++;
                                    rightPos[i].gameObject.SetActive(false);
                                }
                                else
                                {
                                    AudioManager.Play("pen_fail");
                                    anim.SetTrigger("fail");
                                    transform.DOMove(new Vector3(oldPosition.x, oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        AudioManager.Play("pen_fail");
                        anim.SetTrigger("fail");
                        transform.DOMove(new Vector3(oldPosition.x, oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);
                    }
                    boxCollider.size = new Vector2(3.28f, 6.84f);
                    isDrag = false;
                    isTouch = 0;
                }
            }
        }
        if (!locked && time >= 5 && !gameModeToturial.win)
        {
            AudioManager.Play("change_sprite");
            anim.SetTrigger("change");
            transform.DOMove(new Vector3(oldPosition.x, oldPosition.y, 0), 0.5f).SetEase(Ease.OutSine);
            time = 0;
        }
    }
    bool IsWithinBoxCollider()
    {
        Vector3 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 touchPosition2D = new Vector2(touchPosition.x, touchPosition.y);
        return boxCollider.OverlapPoint(touchPosition2D);
    }
    public void ChangeSprite()
    {
        playerRenderer.sprite = gameModeToturial.playerSprites[gameModeToturial.numbers[numberSprite]];
    }
}
