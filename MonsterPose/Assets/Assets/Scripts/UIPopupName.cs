using Dan.Main;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupName : MonoBehaviour
{
    public InputField nameUser;
    public HomeController home;

    [HideInInspector]
    public int idProfile;

    private Animator anim;
    // Start is called before the first frame update
    void OnEnable()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("show");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Done()
    {
        if(nameUser.text.Length >=3 && nameUser.text.Length <= 10)
        {
            PlayerPrefs.SetString("name", nameUser.text);
            StartCoroutine(EffectDone());
        }
    }
    IEnumerator EffectDone()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(0.8f);
        if(idProfile == 1)
        {
            home.Profile();
            gameObject.SetActive(false);
        }
        if(idProfile == 2)
        {
            home.LeaderBoard();
            LeaderboardCreator.UploadNewEntry(LeaderBoardMain.publicKey,
                    PlayerPrefs.GetString("name"),
                    PlayerPrefs.GetInt("score"),
                    PlayerPrefs.GetInt("avatar").ToString()
                    );
            gameObject.SetActive(false);
        }
    }
    public void Exit()
    {
        StartCoroutine(EffectExit());
    }
    IEnumerator EffectExit()
    {
        anim.SetTrigger("hide");
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
    }
}
