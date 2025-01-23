using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private Light spotlight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 1f;
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    private void Start()
    {
        spotlight = GetComponent<Light>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            float targetIntensity = Random.Range(minIntensity, maxIntensity);
            float currentIntensity = spotlight.intensity;

            float elapsedTime = 0f;
            while (elapsedTime < flickerSpeed)
            {
                spotlight.intensity = Mathf.Lerp(currentIntensity, targetIntensity, elapsedTime / flickerSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            spotlight.intensity = targetIntensity;
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime)); // Wait for a random time before flickering again
        }
    }
}