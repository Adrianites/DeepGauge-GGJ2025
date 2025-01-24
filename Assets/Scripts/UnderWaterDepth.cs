using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UnderWaterDepth : MonoBehaviour
{
    [Header("Depth Parameters")]
    [SerializeField] private Transform CameraTransform;
        [SerializeField] private Camera CameraObj;
    [SerializeField] private float Depth;
    [SerializeField] private PostProcessVolume PostProcessVolume;

    [Header("PP Profiles")]
    [SerializeField] private PostProcessProfile ShallowPP;
    [SerializeField] private PostProcessProfile DeepPP;

    [Header("Customisation")]
    [SerializeField] private float ShallowFogEndDistance = 25f;
    [SerializeField] private Color ShallowFogColor = new Color(13f / 255f, 13f / 255f, 31f / 255f, 255f / 255f);
    [SerializeField] private float DeepFogEndDistance = 50f;
    [SerializeField] private Color DeepFogColor = new Color(23f / 255f, 85f / 255f, 135f / 255f, 255f / 255f);

    private void Update()
    {
        float t = Mathf.InverseLerp(Depth, Depth - -10f, CameraTransform.position.y);
        BlendEffects(t);
    }

    private void BlendEffects(float t)
    {
        RenderSettings.fogEndDistance = Mathf.Lerp(ShallowFogEndDistance, DeepFogEndDistance, t);
        RenderSettings.fogColor = Color.Lerp(ShallowFogColor, DeepFogColor, t);
        CameraObj.backgroundColor = Color.Lerp(ShallowFogColor, DeepFogColor, t);
        PostProcessVolume.profile = t > 0.5f ? DeepPP : ShallowPP;
    }
}