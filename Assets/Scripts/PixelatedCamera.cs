using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class PixelatedCamera : MonoBehaviour
{
    public int targetHeight = 240;

    private Camera cam;
    private Camera displayCamera;
    private CommandBuffer displayCommands;
    private RenderTexture lowResTexture;
    private int screenWidth;
    private int screenHeight;
    private int currentTargetHeight;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        GameObject display = new GameObject("Pixelated Display");
        display.transform.SetParent(transform, false);
        displayCamera = display.AddComponent<Camera>();
        displayCamera.cullingMask = 0;
        displayCamera.clearFlags = CameraClearFlags.SolidColor;
        displayCamera.backgroundColor = Color.black;
        displayCamera.depth = cam.depth + 1;
        displayCamera.allowHDR = false;
        displayCamera.allowMSAA = false;

        displayCommands = new CommandBuffer { name = "Pixelated Display" };
        displayCamera.AddCommandBuffer(CameraEvent.AfterEverything, displayCommands);

        Resize();
    }

    private void OnDisable()
    {
        cam.targetTexture = null;
        ReleaseTexture();

        if (displayCamera != null)
        {
            Destroy(displayCamera.gameObject);
        }
        displayCommands.Release();
        screenWidth = 0;
        screenHeight = 0;
    }

    private void LateUpdate()
    {
        if (Screen.width != screenWidth || Screen.height != screenHeight || targetHeight != currentTargetHeight)
        {
            Resize();
        }
    }

    private void Resize()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        currentTargetHeight = targetHeight;

        int blockSize = Mathf.Max(1, Mathf.RoundToInt((float)screenHeight / Mathf.Max(1, targetHeight)));
        int width = Mathf.CeilToInt((float)screenWidth / blockSize);
        int height = Mathf.CeilToInt((float)screenHeight / blockSize);

        cam.targetTexture = null;
        ReleaseTexture();
        lowResTexture = new RenderTexture(width, height, 24);
        lowResTexture.filterMode = FilterMode.Point;
        lowResTexture.antiAliasing = 1;
        lowResTexture.Create();
        cam.targetTexture = lowResTexture;

        Vector2 scale = new Vector2((float)screenWidth / (width * blockSize), (float)screenHeight / (height * blockSize));
        displayCommands.Clear();
        displayCommands.Blit(lowResTexture, BuiltinRenderTextureType.CameraTarget, scale, Vector2.zero);
    }

    private void ReleaseTexture()
    {
        if (lowResTexture != null)
        {
            lowResTexture.Release();
            Destroy(lowResTexture);
            lowResTexture = null;
        }
    }
}
