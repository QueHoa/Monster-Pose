using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneHit.Framework;
using DG.Tweening;

public class GameModeTutorial : MonoBehaviour
{
    public Sprite[] playerSprites;
    public int numberLockWin;
    [HideInInspector]
    public List<int> numbers = new List<int>();
    [HideInInspector]
    public int numberLock;
    [HideInInspector]
    public bool isTutorial;
    [HideInInspector]
    public bool win;
    private ModeController mode;
    private GameObject endGameMode;
    private Animator anim;
    private float time;

    private void OnEnable()
    {
        AudioManager.Play("new_level");
        mode = GameManager.Instance.modeController;
        endGameMode = GameManager.Instance.endGameMode;
        anim = GetComponent<Animator>(); 
        StartCoroutine(Tutorial());
        for (int i = 0; i < playerSprites.Length; i++)
        {
            numbers.Add(i);
        }
        isTutorial = true;
        numberLock = 0;
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (!isTutorial)
        {
            time += Time.deltaTime;
        }
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
    IEnumerator Tutorial()
    {
        anim.SetTrigger("tutorial");
        Input.multiTouchEnabled = false;
        yield return new WaitForSeconds(13f);
        Input.multiTouchEnabled = true;
        isTutorial = false;
    }
    IEnumerator Win()
    {
        mode.isWin = true;
        yield return new WaitForSeconds(0.3f);
        endGameMode.SetActive(true);
    }
}
