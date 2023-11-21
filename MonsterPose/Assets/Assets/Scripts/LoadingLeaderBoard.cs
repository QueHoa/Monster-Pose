using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingLeaderBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(EffectOff());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator EffectOff()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
