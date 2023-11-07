using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        StartCoroutine(EffectStart());
    }
    IEnumerator EffectStart()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.gameObject.SetActive(false);
    }
}
