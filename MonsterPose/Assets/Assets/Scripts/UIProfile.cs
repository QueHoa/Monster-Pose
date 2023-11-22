using Dan.Main;
using OneHit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour
{
    public Image avatar;
    public InputField nameUser;
    public Sprite[] listAvt;

    [HideInInspector]
    public int idAvatar;

    private Animator anim;
    private int idAvt;
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("show");
        idAvt = PlayerPrefs.GetInt("avatar");
        idAvatar = idAvt;
        if(idAvt >= 0 && idAvt < 8)
        {
            avatar.sprite = listAvt[idAvt];
        }
        nameUser.text = PlayerPrefs.GetString("name");
    }

    // Update is called once per frame
    void Update()
    {
        idAvt = idAvatar;
        if (idAvt >= 0 && idAvt < 8)
        {
            avatar.sprite = listAvt[idAvt];
        }
    }
    public void Save()
    {
        if(nameUser.text.Length >=3 && nameUser.text.Length <= 10)
        {
            StartCoroutine(EffectSave());
        }
    }
    IEnumerator EffectSave()
    {
        AudioManager.Play("click");
        PlayerPrefs.SetInt("avatar", idAvatar);
        PlayerPrefs.SetString("name", nameUser.text);
        LeaderboardCreator.UploadNewEntry(LeaderBoardMain.publicKey,
                    PlayerPrefs.GetString("name"),
                    PlayerPrefs.GetInt("score"),
                    PlayerPrefs.GetInt("avatar").ToString()
                    );
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
