using System;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BarcodeCam : MonoBehaviour
{
    public ARCameraManager cameraManager; // ARCameraManager to manage ARKit camera frames
    
    private Texture2D cameraTexture;

    private void OnEnable()
    {
        // Subscribe to the frameReceived event to get the camera frame from ARKit
        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        // Unsubscribe from the frameReceived event
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs args)
    {
        if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            // Convert XRCpuImage to Texture2D to display or use it
          
            UpdateCameraTexture(image);
            image.Dispose();  // Dispose of the image to avoid memory leaks
        }
    }

    private void UpdateCameraTexture(XRCpuImage image)
    {
        // Create a new texture if necessary
        if (cameraTexture == null || cameraTexture.width != image.width || cameraTexture.height != image.height)
        {
            cameraTexture = new Texture2D(image.width, image.height, TextureFormat.RGBA32, false);
        }

        // Convert the image to raw texture data (RGBA format)
        XRCpuImage.ConversionParams conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width, image.height),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // Allocate a buffer to store the image
        var rawTextureData = cameraTexture.GetRawTextureData<byte>();
        image.Convert(conversionParams, rawTextureData);
        cameraTexture.Apply();
        ScanBarcodeFromTexture();

    }

    private void ScanBarcodeFromTexture()
    {
        var barcodeReader = new ZXing.BarcodeReader() { AutoRotate = true };
        Color32[] pixels = cameraTexture.GetPixels32();
        try
        {
            // decode the current frame
            var result = barcodeReader.Decode(pixels, cameraTexture.width, cameraTexture.height);
            if (result != null)
            {
                Debug.Log("barcode 4 " + result.Text);
            }

            // Sleep a little bit and set the signal to get the next frame
            //Thread.Sleep(200);
            //pixels = null;
        }
        catch
        {
        }
    }

}

