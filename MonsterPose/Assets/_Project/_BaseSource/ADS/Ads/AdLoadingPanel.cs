using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdLoadingPanel : MonoBehaviour
{
    public static AdLoadingPanel Instance;

    public GameObject panel;
    public void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }
    public void TurnOn()
    {
        panel.SetActive(true);
    }
    public void TurnOff()
    {
        panel.SetActive(false);
    }
}
