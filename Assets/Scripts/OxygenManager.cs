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
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
        }
        else
        {
            Destroy(gameObject);
        }
        Initialize();
    }

    private void Start()
    {
        DeathCanvasNbubbles();
    }

    private void Initialize()
    {
        Debug.Log("Initializing OxygenManager...");

        o2Bar = GameObject.Find("O2Bar")?.GetComponent<Slider>();
        fadeImage = GameObject.Find("FadeImage")?.GetComponent<Image>();
        deathUI = GameObject.FindWithTag("DeathUI");
        inGameUI = GameObject.Find("InGameCanvas");
        bubblesParticleSystem = GameObject.FindWithTag("Bubbles");

        if (o2Bar != null)
        {
            currentOxygen = maxOxygen;
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

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource added.");
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");
        Initialize(); // Reinitialize references when a new scene is loaded

        // Adjust oxygen depletion rate based on the scene
        if (scene.name == "Level2" || scene.name == "Level3")
        {
            oxygenDepletionRate = 1f;
            Debug.Log("Oxygen depletion rate set to 1 for Level2 or Level3.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from the sceneLoaded event
    }

    private void Update()
    {
        if (!isFading)
        {
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;
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
        if (o2Bar != null)
        {
            o2Bar.value = currentOxygen;
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