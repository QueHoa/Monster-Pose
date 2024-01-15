using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OneHit.Framework;

public class ScreenshotWin : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public static void Screenshot(Camera screenshotCamera, RawImage rawImage)
    {
        AudioManager.Play("camera");
        int captureWidth = Screen.width;
        int captureHeight = Screen.height;
        int cropWidth = captureWidth;
        int cropHeight = captureWidth + 100;
        // Chụp màn hình
        RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);
        screenshotCamera.targetTexture = rt;
        screenshotCamera.Render();
        RenderTexture.active = rt;

        // Tạo Texture2D từ RenderTexture
        Texture2D capturedTexture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        capturedTexture.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
        capturedTexture.Apply();

        int centerX = captureWidth / 2;
        int centerY = captureHeight / 2;
        int cropX = centerX - cropWidth / 2;
        int cropY = 150 + centerY - cropHeight / 2;
        // Cắt ảnh chụp màn hình theo kích cỡ
        Color[] pixels = capturedTexture.GetPixels(cropX, cropY, cropWidth, cropHeight);
        Texture2D croppedTexture = new Texture2D(cropWidth, cropHeight);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        // Hiển thị ảnh đã cắt trên Raw Image
        rawImage.texture = croppedTexture;

        // Giải phóng bộ nhớ
        RenderTexture.active = null;
        screenshotCamera.targetTexture = null;
        rt.Release();
        Destroy(capturedTexture);
    }
}
