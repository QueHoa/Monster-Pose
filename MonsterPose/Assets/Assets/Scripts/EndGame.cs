using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;
using OneHit;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private MainController main;
    [SerializeField]
    private GameObject trailerMode;
    [SerializeField]
    private GameObject[] effect;
    public GameObject fade;
    private RawImage screenShot;
    private Animator anim;
    private int unlockedLevelsNumber;
    private int unlockedModeNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_WIN_" + (main.numberPlaying + 1));
        screenShot = GameManager.Instance.screenShot;
        anim = GetComponent<Animator>();
        anim.SetTrigger("show");
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(true);
        }
        if (main.numberPlaying <= unlockedLevelsNumber - 1)
        {
            Texture2D rawImageTexture = (Texture2D)screenShot.texture;
            byte[] bytes = rawImageTexture.EncodeToJPG(50); // Chuyển texture thành dãy byte JPG
            string filePath = Application.persistentDataPath + "/" + (main.numberPlaying + 1).ToString() + ".jpg";
            File.WriteAllBytes(filePath, bytes); // Lưu dãy byte vào tệp
        }
    }
    public void Clap()
    {
        AudioManager.Play("clap2");
    }
    // Update is called once per frame
    void Update()
    {
        if (main.numberPlaying == unlockedLevelsNumber - 1 && main.numberPlaying != 19)
        {
            PlayerPrefs.SetInt("levelsUnlocked", unlockedLevelsNumber + 1);
        }
    }
    public void Next()
    {
        AudioManager.Play("click");
        AudioManager.Stop("clap2");
        if(main.numberPlaying > 1)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                main.isWin = false;
                Transform Level = main.transform.Find(main.numberPlaying.ToString() + "(Clone)");
                if (Level != null)
                {
                    Destroy(Level.gameObject);
                }
                if (main.numberPlaying != 149)
                {
                    main.numberPlaying++;
                }
                else
                {
                    main.numberPlaying = 0;
                }
                main.gameObject.SetActive(false);
                if (unlockedLevelsNumber > 9 && unlockedLevelsNumber % 10  == 0 && unlockedModeNumber == 1)
                {
                    StartCoroutine(ShowTrailer());
                }
                else
                {
                    StartCoroutine(Hide());
                }
            });
        }
        else
        {
            main.isWin = false;
            Transform Level = main.transform.Find(main.numberPlaying.ToString() + "(Clone)");
            if (Level != null)
            {
                Destroy(Level.gameObject);
            }
            if (main.numberPlaying != 149)
            {
                main.numberPlaying++;
            }
            else
            {
                main.numberPlaying = 0;
            }
            main.gameObject.SetActive(false);
            if (unlockedLevelsNumber > 9 && unlockedLevelsNumber % 10 == 0 && unlockedModeNumber == 1)
            {
                StartCoroutine(ShowTrailer());
            }
            else
            {
                StartCoroutine(Hide());
            }
        }
        
    }
    IEnumerator Hide()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(1.5f);
        fade.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject loadedPrefab = Resources.Load<GameObject>(main.numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, main.transform);
        level.transform.SetParent(main.transform, false);
        main.gameObject.SetActive(true);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
    IEnumerator ShowTrailer()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        trailerMode.SetActive(true);
        gameObject.SetActive(false);
    }
}
