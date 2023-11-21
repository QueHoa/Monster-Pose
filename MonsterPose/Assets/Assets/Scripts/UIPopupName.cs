using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupName : MonoBehaviour
{
    public InputField nameUser;

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
            StartCoroutine(EffectExit());
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
