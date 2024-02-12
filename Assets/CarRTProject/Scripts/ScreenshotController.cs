using UnityEngine;

[SerializeField]
public class ScreenshotController : MonoBehaviour
{
    // Reference to the MainCamera object
    private Camera mainCamera;
    private int resWidth = 50;
    private int resHeight = 50;

    // Start is called before the first frame update
    void Start()
    {
        // Find the MainCamera object in the scene
        mainCamera = Camera.main;

        // Check if MainCamera object exists
        if (mainCamera == null)
        {
            Debug.LogError("MainCamera not found in the scene.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Capture the screenshot and get the byte array
        byte[] screenshotBytes = CaptureScreenshot();

        // Check if the screenshot was captured successfully
        if (screenshotBytes != null)
        {
            // Process the screenshot bytes (e.g., send them over the network, etc.)
            // Debug.Log("Screenshot captured. Byte array length: " + screenshotBytes.Length);
            string base64Screenshot = System.Convert.ToBase64String(screenshotBytes);
            // Debug.Log("Screenshot captured. Byte array length: " + base64Screenshot);
        }
        else
        {
            Debug.LogError("Failed to capture screenshot.");
        }
    }

    // Method to capture the screenshot from the MainCamera object
    public byte[] CaptureScreenshot()
    {
        // Check if MainCamera object exists
        if (mainCamera == null)
        {
            Debug.LogError("MainCamera not found.");
            return null;
        }

        // Capture the screenshot
        // Debug.Log($"{Screen.width} and {Screen.height}");
        RenderTexture renderTexture = new RenderTexture(resWidth, resHeight, 8);
        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();
        RenderTexture.active = renderTexture;
        Texture2D screenshotTexture = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        screenshotTexture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenshotTexture.Apply();
        RenderTexture.active = null;

        // Unset the target texture of the camera
        mainCamera.targetTexture = null;

        // Convert the texture to PNG byte array
        byte[] screenshotBytes = screenshotTexture.EncodeToPNG();

        // Destroy temporary objects
        DestroyImmediate(renderTexture);
        DestroyImmediate(screenshotTexture);

        return screenshotBytes;
    }
}
