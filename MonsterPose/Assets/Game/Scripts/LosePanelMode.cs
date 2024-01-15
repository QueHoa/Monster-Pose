using OneHit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanelMode : MonoBehaviour
{
    public ModeController mode;
    private Animator anim;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        FirebaseManager.Instance.LogEvent("LEVEL_LOSE_CHALLENGE_" + mode.numberPlaying);
        anim.SetTrigger("show");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
