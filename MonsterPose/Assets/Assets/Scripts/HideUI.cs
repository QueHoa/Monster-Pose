using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    public GameObject bg;
    public GameObject[] ui;
    private bool onBg;
    private bool onUi;
    // Start is called before the first frame update
    void Start()
    {
        onBg = true;
        onUi = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UI()
    {
        if (onUi)
        {
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].SetActive(false);
            }
            onUi = false;
        }
        else
        {
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].SetActive(true);
            }
            onUi = true;
        }
    }
    public void BG()
    {
        if (onBg)
        {
            bg.SetActive(false);
            onBg = false;
        }
        else
        {
            bg.SetActive(true);
            onBg = true;
        }
    }
}
