using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;

public class EndGameMode : MonoBehaviour
{
    [SerializeField]
    private ModeController mode;
    [SerializeField]
    private GameObject[] effect;
    private int unlockedModeNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (mode.numberPlaying == unlockedModeNumber)
        {
            PlayerPrefs.SetInt("levelsModeUnlocked", unlockedModeNumber + 1);
        }
    }
    public void Next()
    {
        AudioManager.Play("click");
        mode.isWin = false;
        Transform Level = mode.transform.Find("Lv" + mode.numberPlaying.ToString() + "(Clone)");
        if (Level != null)
        {
            Destroy(Level.gameObject);
        }
        mode.gameObject.SetActive(false);
        mode.numberPlaying++;
        StartCoroutine(Hide());
    }
    IEnumerator Hide()
    {
        GameObject loadedPrefab = Resources.Load<GameObject>("Lv" + mode.numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, mode.transform);
        level.transform.SetParent(mode.transform, false);
        mode.gameObject.SetActive(true);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
}
