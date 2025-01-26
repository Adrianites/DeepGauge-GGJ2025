using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class OxygenManager : MonoBehaviour
{
    public Slider o2Bar;
    public Image fadeImage;
    public GameObject bubblesParticleSystem;
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 1f;
    public float fadeDuration = 5f;
    public float secondsPerBubble = 2f;

    public GameObject deathUI;
    public GameObject inGameUI;

    public UnityEvent OnPlayerDeath;

    private float currentOxygen;
    private bool isFading = false;
    private float fadeTimer = 0f;
    private float bubblesTimer = 0f;
    private bool bubblesShown = false;
    private AudioSource audioSource;

    public static OxygenManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentOxygen = maxOxygen;
        o2Bar.maxValue = maxOxygen;
        o2Bar.value = currentOxygen;
        fadeImage.color = new Color(0, 0, 0, 0);
        audioSource = gameObject.AddComponent<AudioSource>();

        if (OnPlayerDeath == null)
        {
            OnPlayerDeath = new UnityEvent();
        }

        if (bubblesParticleSystem != null)
        {
            bubblesParticleSystem.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isFading)
        {
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;
            o2Bar.value = currentOxygen;

            if (currentOxygen <= 0)
            {
                StartFade();
            }

            bubblesTimer += Time.deltaTime;
            if (bubblesTimer >= secondsPerBubble)
            {
                ToggleBubbles();
                bubblesTimer = 0f;
            }
        }
        else
        {
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);

            if (alpha >= 1)
            {
                OnFadeComplete();
            }
        }
    }

    private void StartFade()
    {
        isFading = true;
        fadeTimer = 0f;
    }

    private void OnFadeComplete()
    {
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }

        if (inGameUI != null)
        {
            inGameUI.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        OnPlayerDeath?.Invoke();
    }

    private void ToggleBubbles()
    {
        bubblesShown = !bubblesShown;
        if (bubblesParticleSystem != null)
        {
            bubblesParticleSystem.SetActive(bubblesShown);
        }
        if (audioSource != null)
        {
            if (bubblesShown)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    public void RecoverOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0, maxOxygen);
        o2Bar.value = currentOxygen;
    }
}