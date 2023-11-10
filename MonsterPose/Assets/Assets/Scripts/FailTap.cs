using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailTap : MonoBehaviour
{
    public HapticTypes hapticTypes = HapticTypes.Failure;
    private bool hapticsAllowed = true;
    private int isVibrate;
    // Start is called before the first frame update
    void Start()
    {
        MMVibrationManager.SetHapticsActive(hapticsAllowed);
        isVibrate = PlayerPrefs.GetInt("VibrateOn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Fail()
    {
        if (isVibrate == 1)
        {
            MMVibrationManager.Haptic(hapticTypes, true, true, this);
        }
    }
}
