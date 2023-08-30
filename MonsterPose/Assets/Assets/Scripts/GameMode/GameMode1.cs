using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneHit.Framework;
using DG.Tweening;

public class GameMode1 : MonoBehaviour
{
    public Sprite[] playerSprites;
    public RectTransform rightPos;
    public int numberLockWin;
    [HideInInspector]
    public List<int> numbers = new List<int>();
    [HideInInspector]
    public int numberLock;
    [HideInInspector]
    public bool win;
    private ModeController mode;
    private GameObject endGameMode;
    private float time;

    private void OnEnable()
    {
        AudioManager.Play("new_level");
        mode = GameManager.Instance.modeController;
        endGameMode = GameManager.Instance.endGameMode;
        rightPos.anchoredPosition = new Vector2(0, -800);
        for (int i = 0; i < playerSprites.Length; i++)
        {
            numbers.Add(i);
        }
        numberLock = 0;
        rightPos.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutQuart);
        ShuffleList();
    }
    void Start()
    {
        
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 5)
        {
            ShuffleList();
            time = 0;
        }
        if(numberLock == numberLockWin)
        {
            win = true;
            StartCoroutine(Win());
            numberLock = 0;
        }
    }
    private void ShuffleList()
    {
        for (int i = 0; i < numbers.Count; i++)
        {
            int temp = numbers[i];
            int randomIndex = Random.Range(i, numbers.Count);
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }
    }
    IEnumerator Win()
    {
        mode.isWin = true;
        yield return new WaitForSeconds(0.3f);
        endGameMode.SetActive(true);
    }
}
