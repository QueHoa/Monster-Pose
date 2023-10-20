using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Extension
{
    public static void Fade(this Image image, float alpha)
    {
        var color = image.color;
        color.a = alpha;
        image.color = color;
    }
    public static int Percent(this float x)
    {
        return (100 * x).Int();
    }
    public static int Millisecond(this float second)
    {
        return Mathf.FloorToInt(second * 1000);
    }

    public static int Int(this float x)
    {
        return Mathf.FloorToInt(x);
    }

    public static int Int(this TextMeshProUGUI text)
    {
        int x = 0;
        try { x = float.Parse(text.text).Int(); }
        catch (FormatException e) { Debug.LogError(e); }
        return x;
    }
}
