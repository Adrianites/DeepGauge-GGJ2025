using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OxygenManager : MonoBehaviour
{
    public Slider o2Bar; // Reference to the UI Slider component for the O2 bar
    public Image fadeImage; // Reference to the UI Image component for the screen fade
    public ParticleSystem bubblesParticleSystem; // Reference to the Particle System for the bubbles
    public AudioClip bubblesAudioClip; // Audio clip to play when bubbles appear
    public float maxOxygen = 100f; // Maximum oxygen level
    public float oxygenDepletionRate = 1f; // Rate at which oxygen depletes per second
    public float fadeDuration = 5f; // Duration for the screen to fade to black
    public float bubblesDelay = 10f; // Time before bubbles first appear
    public float bubblesDuration = 2f; // Duration for which bubbles are visible
    public float bubblesInterval = 5f; // Interval between bubble appearances

    private float currentOxygen;
    private bool isFading = false;
    private float fadeTimer = 0f;
    private float bubblesTimer = 0f;
    private bool bubblesShown = false;
    private AudioSource audioSource;

    private void Start()
    {
        currentOxygen = maxOxygen;
        o2Bar.maxValue = maxOxygen;
        o2Bar.value = currentOxygen;
        fadeImage.color = new Color(0, 0, 0, 0); // Ensure the fade image is initially transparent
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isFading)
        {
            // Deplete oxygen over time
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;
            o2Bar.value = currentOxygen;

            // Check if oxygen has reached zero
            if (currentOxygen <= 0)
            {
                StartFade();
            }

            // Handle bubbles appearance
            bubblesTimer += Time.deltaTime;
            if (bubblesTimer >= bubblesDelay && !bubblesShown)
            {
                ShowBubbles();
            }
            else if (bubblesShown && bubblesTimer >= bubblesDelay + bubblesDuration)
            {
                HideBubbles();
            }
            else if (!bubblesShown && bubblesTimer >= bubblesDelay + bubblesDuration + bubblesInterval)
            {
                bubblesTimer = 0f; // Reset the timer to show bubbles again
            }
        }
        else
        {
            // Handle screen fading
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);

            // Optionally, you can reload the scene or handle game over logic here
            if (alpha >= 1)
            {
                // Reload the scene or handle game over
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private void StartFade()
    {
        isFading = true;
        fadeTimer = 0f;
    }

    private void ShowBubbles()
    {
        Debug.Log("Showing bubbles");
        bubblesShown = true;
        bubblesParticleSystem.Play(); // Play the particle system
        audioSource.clip = bubblesAudioClip;
        audioSource.Play();
    }

    private void HideBubbles()
    {
        Debug.Log("Hiding bubbles");
        bubblesShown = false;
        bubblesParticleSystem.Stop(); // Stop the particle system
        audioSource.Stop(); // Stop the audio
    }
}