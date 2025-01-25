using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeMagnitude = 0.05f;
    public float shakeFrequency = 1.0f;

    private Vector3 initialPosition;

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        float shakeX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2 - 1;
        float shakeY = Mathf.PerlinNoise(0f, Time.time * shakeFrequency) * 2 - 1;
        transform.localPosition = initialPosition + new Vector3(shakeX, shakeY, 0) * shakeMagnitude;
    }
}