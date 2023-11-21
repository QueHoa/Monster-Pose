using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProfile : MonoBehaviour
{
    public Image avatar;
    public InputField nameUser;
    public GameObject popupName;
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
        if (nameUser.text == "")
        {
            popupName.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        idAvt = idAvatar;
        if (idAvt >= 0 && idAvt < 8)
        {
            avatar.sprite = listAvt[idAvt];
        }
        if (!popupName.activeInHierarchy && PlayerPrefs.GetString("name") == "")
        {
            nameUser.text = PlayerPrefs.GetString("name");
        }
    }
    public void Save()
    {
        StartCoroutine(EffectSave());
    }
    IEnumerator EffectSave()
    {
        anim.SetTrigger("hide");
        PlayerPrefs.SetInt("avatar", idAvatar);
        PlayerPrefs.SetString("name", nameUser.text);
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
