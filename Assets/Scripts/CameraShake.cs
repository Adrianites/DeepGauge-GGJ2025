using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeMagnitude = 0.05f; // Reduced magnitude for subtle effect
    public float shakeFrequency = 1.0f; // Frequency of the shake

    private Vector3 initialPosition;

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // Apply continuous subtle shake effect
        float shakeX = Mathf.PerlinNoise(Time.time * shakeFrequency, 0f) * 2 - 1;
        float shakeY = Mathf.PerlinNoise(0f, Time.time * shakeFrequency) * 2 - 1;
        transform.localPosition = initialPosition + new Vector3(shakeX, shakeY, 0) * shakeMagnitude;
    }
}