using UnityEngine;
using UnityEngine.UI;
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
    private bool isDead = false;
    private float fadeTimer = 0f;
    private float bubblesTimer = 0f;
    private bool bubblesShown = false;

    public static OxygenManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Start()
    {
        DeathCanvasNbubbles();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Initialize()
    {
        Debug.Log("Initializing OxygenManager...");

        if (o2Bar == null)
        {
            o2Bar = GameObject.Find("O2Bar")?.GetComponent<Slider>();
        }
        if (fadeImage == null)
        {
            fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();
        }
        if (deathUI == null)
        {
            deathUI = GameObject.FindWithTag("DeathUI");
        }
        if (inGameUI == null)
        {
            inGameUI = GameObject.Find("InGameCanvas");
        }
        if (bubblesParticleSystem == null)
        {
            bubblesParticleSystem = GameObject.FindWithTag("Bubbles");
        }

        currentOxygen = maxOxygen;
        isFading = false;
        isDead = false;
        fadeTimer = 0f;
        bubblesTimer = 0f;
        bubblesShown = false;

        if (o2Bar != null)
        {
            o2Bar.maxValue = maxOxygen;
            o2Bar.value = currentOxygen;
            Debug.Log("O2Bar found and initialized.");
        }
        else
        {
            Debug.LogError("O2Bar not found.");
        }

        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            Debug.Log("FadeImage found and initialized.");
        }
        else
        {
            Debug.LogError("FadeImage not found.");
        }

        if (OnPlayerDeath == null)
        {
            OnPlayerDeath = new UnityEvent();
            Debug.Log("OnPlayerDeath event initialized.");
        }

        if (bubblesParticleSystem != null)
        {
            bubblesParticleSystem.SetActive(false);
            Debug.Log("BubblesParticleSystem found and initialized.");
        }
        else
        {
            Debug.LogError("BubblesParticleSystem not found.");
        }

        if (deathUI != null)
        {
            Debug.Log("Death UI found and initialized.");
            //DeathCanvasNbubbles();
        }
        else
        {
            Debug.LogError("Death UI not found. Ensure the GameObject is tagged correctly and is active in the scene.");
        }

        if (inGameUI != null)
        {
            Debug.Log("InGameCanvas found and initialized.");
        }
        else
        {
            Debug.LogError("InGameCanvas not found.");
        }
    }

    private void DeathCanvasNbubbles()
    {
        if (deathUI != null)
        {
            deathUI.SetActive(false);
        }

        if (bubblesParticleSystem != null)
        {
            bubblesParticleSystem.SetActive(false);
        }
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (!isFading)
        {
            currentOxygen = Mathf.Max(currentOxygen - oxygenDepletionRate * Time.deltaTime, 0f);
            if (o2Bar != null)
            {
                o2Bar.value = currentOxygen;
            }

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
            if (fadeImage != null)
            {
                fadeImage.color = new Color(0, 0, 0, alpha);
            }

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

    private void StopFade()
    {
        isFading = false;
        fadeTimer = 0f;
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    private void OnFadeComplete()
    {
        isDead = true;

        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }

        if (inGameUI != null)
        {
            inGameUI.SetActive(false);
        }

        if (bubblesParticleSystem != null)
        {
            bubblesParticleSystem.SetActive(false);
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
    }

    public void RecoverOxygen(float amount)
    {
        if (isDead)
        {
            return;
        }

        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0, maxOxygen);
        if (o2Bar != null)
        {
            o2Bar.value = currentOxygen;
        }

        if (isFading && currentOxygen > 0)
        {
            StopFade();
        }
    }

    public float GetCurrentOxygen()
    {
        return currentOxygen;
    }

    public void SetCurrentOxygen(float newOxygen)
    {
        currentOxygen = Mathf.Clamp(newOxygen, 0, maxOxygen);
        if (o2Bar != null)
        {
            o2Bar.value = currentOxygen;
        }
    }
}
