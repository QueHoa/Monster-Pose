using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;

public class TrailerMode : MonoBehaviour
{
    [SerializeField]
    private MainController main;
    [SerializeField]
    private ModeController mode;
    [SerializeField]
    private GameObject[] effect;
    private Animator anim;
    private int unlockedLevelsNumber;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(true);
        }
        anim.SetTrigger("show");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Next()
    {
        AudioManager.Play("click");
        
        StartCoroutine(NextEffect());
    }
    IEnumerator NextEffect()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(0.4f);
        mode.numberPlaying = 1;
        GameObject loadedPrefab = Resources.Load<GameObject>("Lv1");
        GameObject level = Instantiate(loadedPrefab, mode.transform);
        level.transform.SetParent(mode.transform, false);
        mode.gameObject.SetActive(true);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
    public void NoThank()
    {
        AudioManager.Play("click");
        StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(0.4f);
        GameObject loadedPrefab = Resources.Load<GameObject>(main.numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, main.transform);
        level.transform.SetParent(main.transform, false);
        main.gameObject.SetActive(true);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
