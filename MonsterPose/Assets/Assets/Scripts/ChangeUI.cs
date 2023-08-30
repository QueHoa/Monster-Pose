using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUI : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        StartCoroutine(Off());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Off()
    {
        anim.SetTrigger("show");
        yield return new WaitForSeconds(1.2f);
        gameObject.SetActive(false);
    }
}
