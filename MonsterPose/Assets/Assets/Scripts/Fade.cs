using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(EffectStart());
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator EffectStart()
    {
        yield return new WaitForSeconds(1.34f);
        gameObject.SetActive(false);
    }
}
