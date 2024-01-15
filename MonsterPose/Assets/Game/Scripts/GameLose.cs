using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLose : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        anim.SetTrigger("show");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
