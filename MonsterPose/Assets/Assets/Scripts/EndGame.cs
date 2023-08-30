using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    private MainController main;
    [SerializeField]
    private GameObject trailerMode;
    [SerializeField]
    private GameObject[] effect;
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
        anim = GetComponent<Animator>();
        screenShot = GameManager.Instance.screenShot;
    }
    private void OnEnable()
    {
        unlockedLevelsNumber = PlayerPrefs.GetInt("levelsUnlocked");
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(true);
        }
        anim.SetTrigger("show");
        if (main.numberPlaying <= unlockedLevelsNumber - 1)
        {
            Texture2D rawImageTexture = (Texture2D)screenShot.texture;
            byte[] bytes = rawImageTexture.EncodeToJPG(50); // Chuyển texture thành dãy byte JPG
            string filePath = Application.persistentDataPath + "/" + (main.numberPlaying + 1).ToString() + ".jpg";
            File.WriteAllBytes(filePath, bytes); // Lưu dãy byte vào tệp
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (main.numberPlaying == unlockedLevelsNumber - 1)
        {
            PlayerPrefs.SetInt("levelsUnlocked", unlockedLevelsNumber + 1);
        }
    }
    public void Next()
    {
        AudioManager.Play("click");
        main.isWin = false;
        Transform Level = main.transform.Find(main.numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        main.numberPlaying++;
        main.gameObject.SetActive(false); 
        if (unlockedLevelsNumber > 9 && unlockedLevelsNumber % 5 == 0 && unlockedModeNumber == 1)
        {
            StartCoroutine(ShowTrailer());
        }
        else
        {
            StartCoroutine(Hide());
        }
    }
    IEnumerator Hide()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(0.5f);
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
    IEnumerator ShowTrailer()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        trailerMode.SetActive(true);
        gameObject.SetActive(false);
    }
}
