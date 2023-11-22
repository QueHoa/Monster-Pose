using OneHit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseAvt : MonoBehaviour
{
    public GameObject selecting;
    [SerializeField]
    private int idAvt;
    [SerializeField]
    private UIProfile profile;
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(idAvt == profile.idAvatar)
        {
            selecting.SetActive(true);
        }
        else
        {
            selecting.SetActive(false);
        }
    }
    public void Choose()
    {
        AudioManager.Play("click");
        profile.idAvatar = idAvt;
        selecting.SetActive(true);
    }
}
