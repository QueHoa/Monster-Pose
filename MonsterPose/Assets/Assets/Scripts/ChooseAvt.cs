using OneHit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseAvt : MonoBehaviour
{
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
        
    }
    public void Choose()
    {
        AudioManager.Play("click");
        profile.idAvatar = idAvt;
    }
}
