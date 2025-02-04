﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;
using DG.Tweening;
using OneHit;

public class EndGameMode : MonoBehaviour
{
    [SerializeField]
    private ModeController mode;
    [SerializeField]
    private GameObject[] effect;
    public GameObject home;
    public GameObject loading;
    private Animator anim;
    private int unlockedModeNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_WIN_CHALLENGE_" + mode.numberPlaying);
        anim = GetComponent<Animator>();
        anim.SetTrigger("show");
        unlockedModeNumber = PlayerPrefs.GetInt("levelsModeUnlocked");
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(true);
        }
        AudioManager.Play("win_mode");
    }
    // Update is called once per frame
    void Update()
    {
        if (mode.numberPlaying == unlockedModeNumber && mode.numberPlaying != GameManager.numberMode)
        {
            PlayerPrefs.SetInt("levelsModeUnlocked", unlockedModeNumber + 1);
        }
    }
    public void Next()
    {
        AudioManager.Play("click");
        MasterControl.Instance.ShowInterAd((bool res) =>
        {
            FirebaseManager.Instance.LogEvent("LEVEL_NEXT_CHALLENGE_" + mode.numberPlaying);
            mode.isWin = false;
            Transform Level = mode.transform.Find("Lv" + mode.numberPlaying.ToString() + "(Clone)");
            if (Level != null)
            {
                Destroy(Level.gameObject);
            }
            mode.gameObject.SetActive(false);
            if (mode.numberPlaying != GameManager.numberMode)
            {
                mode.numberPlaying++;
            }
            else
            {
                mode.numberPlaying = 1;
            }
            StartCoroutine(Hide());
        });
        
    }
    IEnumerator Hide()
    {
        anim.SetTrigger("hide");
        loading.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        GameObject loadedPrefab = Resources.Load<GameObject>("Lv" + mode.numberPlaying.ToString());
        GameObject level = Instantiate(loadedPrefab, mode.transform);
        level.transform.SetParent(mode.transform, false);
        mode.gameObject.SetActive(true);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        //yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }
    public void Home()
    {
        AudioManager.Play("click");
        MasterControl.Instance.ShowInterAd((bool res) =>
        {
            mode.isWin = false;
            Transform Level = mode.transform.Find("Lv" + mode.numberPlaying.ToString() + "(Clone)");
            if (Level != null)
            {
                Destroy(Level.gameObject);
            }
            if (mode.numberPlaying != GameManager.numberMode)
            {
                mode.numberPlaying++;
            }
            else
            {
                mode.numberPlaying = 1;
            }
            mode.gameObject.SetActive(false);
            StartCoroutine(HideHome());
        });
    }
    IEnumerator HideHome()
    {
        anim.SetTrigger("hide");
        loading.SetActive(true);
        yield return new WaitForSeconds(0.9f);
        home.SetActive(true);
        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].SetActive(false);
        }
        //yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }
}
