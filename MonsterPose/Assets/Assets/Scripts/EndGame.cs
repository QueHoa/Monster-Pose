using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;
using OneHit;
using DG.Tweening;

public class EndGame : MonoBehaviour
{
    [Header("Trailer Mode Enable")]
    public static bool allowTrailer = true;

    public MainController main;
    public GameObject home;
    [SerializeField]
    private GameObject trailerMode;
    [SerializeField]
    private GameObject[] effect;
    public GameObject loading;
    public Text heart;
    public GameObject fade;
    public Text title;
    private RawImage screenShot;
    private Animator anim;
    private int unlockedLevelsNumber;
    private int unlockedModeNumber;
    private int x;
    private int numberHeart;
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
        heart.gameObject.SetActive(false);
        numberHeart = 0;
        x = Random.Range(1,5);
        if (x == 1)
        {
            title.text = "Great!!!";
        }
        else if (x == 2)
        {
            title.text = "Well Done!";
        }
        else if (x == 3)
        {
            title.text = "Good Job!";
        }
        else if (x == 4)
        {
            title.text = "Amazing!!!";
        }
        else if (x == 5)
        {
            title.text = "Perfect!!!";
        }
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
        if (main.numberPlaying == unlockedLevelsNumber - 1 && main.numberPlaying != GameManager.numberLevel - 1)
        {
            PlayerPrefs.SetInt("levelsUnlocked", unlockedLevelsNumber + 1);
        }
    }
    public void Next()
    {
        AudioManager.Play("click");
        AudioManager.Stop("clap2");
        if(unlockedLevelsNumber > GameManager.levelShowAd - 1)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                main.isWin = false;
                if (main.numberPlaying == 0)
                {
                    if (GameManager.numberMonster == 1)
                    {
                        Transform Level = main.transform.Find("01(Clone)");
                        if (Level != null)
                        {
                            Destroy(Level.gameObject);
                        }
                    }
                    else if (GameManager.numberMonster == 2)
                    {
                        Transform Level = main.transform.Find("02(Clone)");
                        if (Level != null)
                        {
                            Destroy(Level.gameObject);
                        }
                    }
                    else if (GameManager.numberMonster == 3)
                    {
                        Transform Level = main.transform.Find("03(Clone)");
                        if (Level != null)
                        {
                            Destroy(Level.gameObject);
                        }
                    }
                }
                else
                {
                    Transform Level = main.transform.Find(main.numberPlaying.ToString() + "(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
                if (main.numberPlaying != GameManager.numberLevel - 1)
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
                    if (allowTrailer)
                    {
                        StartCoroutine(ShowTrailer());
                    }
                    else
                    {
                        StartCoroutine(Hide());
                    }
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
            if (main.numberPlaying == 0)
            {
                if (GameManager.numberMonster == 1)
                {
                    Transform Level = main.transform.Find("01(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
                else if (GameManager.numberMonster == 2)
                {
                    Transform Level = main.transform.Find("02(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
                else if (GameManager.numberMonster == 3)
                {
                    Transform Level = main.transform.Find("03(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
            }
            else
            {
                Transform Level = main.transform.Find(main.numberPlaying.ToString() + "(Clone)");
                if (Level != null)
                {
                    Destroy(Level.gameObject);
                }
            }
            main.numberPlaying++;
            main.gameObject.SetActive(false);
            StartCoroutine(Hide());
        }
        
    }
    IEnumerator Hide()
    {
        /*anim.SetTrigger("hide");
        yield return new WaitForSeconds(1.5f);
        fade.SetActive(true);
        yield return new WaitForSeconds(1f);*/
        loading.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        if(main.numberPlaying == 0)
        {
            if (GameManager.numberMonster == 1)
            {
                GameObject loadedPrefab = Resources.Load<GameObject>("01");
                GameObject level = Instantiate(loadedPrefab, main.transform);
                level.transform.SetParent(main.transform, false);
            }
            else if (GameManager.numberMonster == 2)
            {
                GameObject loadedPrefab = Resources.Load<GameObject>("02");
                GameObject level = Instantiate(loadedPrefab, main.transform);
                level.transform.SetParent(main.transform, false);
            }
            else if (GameManager.numberMonster == 3)
            {
                GameObject loadedPrefab = Resources.Load<GameObject>("03");
                GameObject level = Instantiate(loadedPrefab, main.transform);
                level.transform.SetParent(main.transform, false);
            }
        }
        else
        {
            GameObject loadedPrefab = Resources.Load<GameObject>(main.numberPlaying.ToString());
            GameObject level = Instantiate(loadedPrefab, main.transform);
            level.transform.SetParent(main.transform, false);
        }
        main.gameObject.SetActive(true);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }
    public void ShareImage()
    {
        AudioManager.Play("click");
        AudioManager.Stop("clap2");
        Texture2D text = (Texture2D)screenShot.texture;
        if (unlockedLevelsNumber > GameManager.levelShowAd - 1)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                if (Application.isMobilePlatform)
                {
                    NativeShare nativeShare = new NativeShare();
                    nativeShare.AddFile(text, (main.numberPlaying + 1).ToString() + ".jpg");
                    nativeShare.Share();
                }
            });
        }
        else
        {
            if (Application.isMobilePlatform)
            {
                NativeShare nativeShare = new NativeShare();
                nativeShare.AddFile(text, (main.numberPlaying + 1).ToString() + ".jpg");
                nativeShare.Share();
            }
        }
    }
    public void BackHome()
    {
        AudioManager.Play("click");
        AudioManager.Stop("clap2");
        if (unlockedLevelsNumber > GameManager.levelShowAd - 1)
        {
            MasterControl.Instance.ShowInterAd((bool res) =>
            {
                main.isWin = false;
                if (main.numberPlaying == 0)
                {
                    if (GameManager.numberMonster == 1)
                    {
                        Transform Level = main.transform.Find("01(Clone)");
                        if (Level != null)
                        {
                            Destroy(Level.gameObject);
                        }
                    }
                    else if (GameManager.numberMonster == 2)
                    {
                        Transform Level = main.transform.Find("02(Clone)");
                        if (Level != null)
                        {
                            Destroy(Level.gameObject);
                        }
                    }
                    else if (GameManager.numberMonster == 3)
                    {
                        Transform Level = main.transform.Find("03(Clone)");
                        if (Level != null)
                        {
                            Destroy(Level.gameObject);
                        }
                    }
                }
                else
                {
                    Transform Level = main.transform.Find(main.numberPlaying.ToString() + "(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
                main.gameObject.SetActive(false);
                StartCoroutine(HideHome());
            });
        }
        else
        {
            main.isWin = false;
            if (main.numberPlaying == 0)
            {
                if (GameManager.numberMonster == 1)
                {
                    Transform Level = main.transform.Find("01(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
                else if (GameManager.numberMonster == 2)
                {
                    Transform Level = main.transform.Find("02(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
                else if (GameManager.numberMonster == 3)
                {
                    Transform Level = main.transform.Find("03(Clone)");
                    if (Level != null)
                    {
                        Destroy(Level.gameObject);
                    }
                }
            }
            else
            {
                Transform Level = main.transform.Find(main.numberPlaying.ToString() + "(Clone)");
                if (Level != null)
                {
                    Destroy(Level.gameObject);
                }
            }
            main.gameObject.SetActive(false);
            StartCoroutine(HideHome());
        }
    }
    IEnumerator HideHome()
    {
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        /*anim.SetTrigger("hide");
        yield return new WaitForSeconds(1.5f);
        fade.SetActive(true);*/
        loading.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        home.SetActive(true);
        //yield return new WaitForSeconds(0.2f);
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
    public void SetHeart()
    {
        int endHeart = 0;
        if(main.numberPlaying < 3)
        {
            endHeart = Random.Range(91, 100);
        }
        else if(main.numberPlaying >= 3 && main.numberPlaying < 20)
        {
            if(main.time >= 15)
            {
                endHeart = Random.Range(91, 100);
            }else if (main.time >=10 && main.time < 15)
            {
                endHeart = Random.Range(81, 90);
            }
            else
            {
                endHeart = Random.Range(70, 80);
            }
        }
        else if (main.numberPlaying >= 20 && main.numberPlaying < 50)
        {
            if (main.time >= 20)
            {
                endHeart = Random.Range(91, 100);
            }
            else if (main.time >= 10 && main.time < 20)
            {
                endHeart = Random.Range(81, 90);
            }
            else
            {
                endHeart = Random.Range(70, 80);
            }
        }
        else if (main.numberPlaying >= 50)
        {
            if (main.time >= 20)
            {
                endHeart = Random.Range(91, 100);
            }
            else if (main.time >= 10 && main.time < 20)
            {
                endHeart = Random.Range(81, 90);
            }
            else
            {
                endHeart = Random.Range(70, 80);
            }
        }

        PlayerPrefs.SetInt(main.numberPlaying.ToString(), endHeart);
        heart.gameObject.SetActive(true);
        DOTween.To(() => numberHeart, x => numberHeart = x, endHeart, 1.5f)
            .OnUpdate(() =>
            {
                heart.text ="LOVE: " + numberHeart.ToString();
            });
    }
}
