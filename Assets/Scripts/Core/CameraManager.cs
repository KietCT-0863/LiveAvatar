using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages webcam initialization and provides camera frames for pose detection
/// </summary>
public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Preferred camera resolution width")]
    [SerializeField] private int cameraWidth = 640;
    
    [Tooltip("Preferred camera resolution height")]
    [SerializeField] private int cameraHeight = 480;
    
    [Tooltip("Target frame rate")]
    [SerializeField] private int targetFPS = 30;
    
    [Tooltip("Device name (leave empty for default camera)")]
    [SerializeField] private string deviceName = "";
    
    [Header("Preview Settings")]
    [Tooltip("Optional RawImage to display camera preview")]
    [SerializeField] private RawImage previewImage;
    
    [Tooltip("Show camera preview")]
    [SerializeField] private bool showPreview = true;
    
    // Private fields
    private WebCamTexture webCamTexture;
    private bool isInitialized = false;
    
    // Public properties
    public bool IsInitialized => isInitialized;
    public WebCamTexture WebCamTexture => webCamTexture;
    public int Width => webCamTexture != null ? webCamTexture.width : 0;
    public int Height => webCamTexture != null ? webCamTexture.height : 0;
    
    void Start()
    {
        InitializeCamera();
    }
    
    /// <summary>
    /// Initialize and start the webcam
    /// </summary>
    public void InitializeCamera()
    {
        // Check if webcam devices are available
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.LogError("[CameraManager] No webcam devices found!");
            return;
        }
        
        // Log available cameras
        Debug.Log($"[CameraManager] Found {WebCamTexture.devices.Length} camera(s):");
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            Debug.Log($"  [{i}] {WebCamTexture.devices[i].name}");
        }
        
        // Create WebCamTexture
        if (string.IsNullOrEmpty(deviceName))
        {
            // Use default camera
            webCamTexture = new WebCamTexture(cameraWidth, cameraHeight, targetFPS);
            Debug.Log($"[CameraManager] Using default camera");
        }
        else
        {
            // Use specified camera
            webCamTexture = new WebCamTexture(deviceName, cameraWidth, cameraHeight, targetFPS);
            Debug.Log($"[CameraManager] Using camera: {deviceName}");
        }
        
        // Start camera
        webCamTexture.Play();
        
        // Wait for camera to start
        StartCoroutine(WaitForCameraStart());
    }
    
    /// <summary>
    /// Wait for camera to initialize and start streaming
    /// </summary>
    private System.Collections.IEnumerator WaitForCameraStart()
    {
        // Wait until camera is playing
        while (!webCamTexture.didUpdateThisFrame)
        {
            yield return null;
        }
        
        isInitialized = true;
        
        Debug.Log($"[CameraManager] Camera initialized successfully");
        Debug.Log($"[CameraManager] Resolution: {webCamTexture.width}x{webCamTexture.height} @ {webCamTexture.requestedFPS}fps");
        
        // Setup preview if enabled
        if (showPreview && previewImage != null)
        {
            SetupPreview();
        }
    }
    
    /// <summary>
    /// Setup camera preview on RawImage
    /// </summary>
    private void SetupPreview()
    {
        if (previewImage != null && webCamTexture != null)
        {
            previewImage.texture = webCamTexture;
            
            // Adjust aspect ratio
            RectTransform rectTransform = previewImage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                float aspectRatio = (float)webCamTexture.width / webCamTexture.height;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x / aspectRatio);
            }
            
            Debug.Log("[CameraManager] Camera preview enabled");
        }
    }
    
    /// <summary>
    /// Get current camera frame as Texture2D
    /// </summary>
    /// <returns>Current frame or null if not initialized</returns>
    public Texture2D GetCurrentFrame()
    {
        if (!isInitialized || webCamTexture == null)
        {
            return null;
        }
        
        // Create Texture2D from WebCamTexture
        Texture2D frame = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGB24, false);
        frame.SetPixels(webCamTexture.GetPixels());
        frame.Apply();
        
        return frame;
    }
    
    /// <summary>
    /// Get camera texture directly (more efficient than GetCurrentFrame)
    /// </summary>
    /// <returns>WebCamTexture or null if not initialized</returns>
    public Texture GetCameraTexture()
    {
        return isInitialized ? webCamTexture : null;
    }
    
    /// <summary>
    /// Stop the camera
    /// </summary>
    public void StopCamera()
    {
        if (webCamTexture != null && webCamTexture.isPlaying)
        {
            webCamTexture.Stop();
            isInitialized = false;
            Debug.Log("[CameraManager] Camera stopped");
        }
    }
    
    /// <summary>
    /// Restart the camera
    /// </summary>
    public void RestartCamera()
    {
        StopCamera();
        InitializeCamera();
    }
    
    /// <summary>
    /// Toggle camera preview visibility
    /// </summary>
    public void TogglePreview()
    {
        showPreview = !showPreview;
        
        if (previewImage != null)
        {
            previewImage.gameObject.SetActive(showPreview);
        }
    }
    
    void OnDestroy()
    {
        // Clean up
        StopCamera();
        
        if (webCamTexture != null)
        {
            Destroy(webCamTexture);
        }
    }
    
    void OnApplicationQuit()
    {
        StopCamera();
    }
}
