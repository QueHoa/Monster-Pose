using OneHit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    public MainController main;
    private Animator anim;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_LOSE_" + (main.numberPlaying + 1));
        anim.SetTrigger("show");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
