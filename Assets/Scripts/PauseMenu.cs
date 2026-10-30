using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject InGameUI;
    public GameObject settingsMenuUI;
    public OxygenManager oxygenManager;
    public bool isDead = false;

    void Start()
    {
        if (settingsMenuUI == null)
        {
            Transform settings = transform.Find("SettingsMenuCanvas");
            if (settings != null)
            {
                settingsMenuUI = settings.gameObject;
            }
        }

        pauseMenuUI.SetActive(false);
        InGameUI.SetActive(true);
        if (settingsMenuUI != null)
        {
            settingsMenuUI.SetActive(false);
        }

        Time.timeScale = 1f;
        LockCursor();

        if (oxygenManager == null)
        {
            oxygenManager = OxygenManager.Instance;
        }

        if (oxygenManager != null)
        {
            oxygenManager.OnPlayerDeath.AddListener(HandlePlayerDeath);
        }
    }

    void Update()
    {
        if (Application.isFocused && !GameIsPaused && !isDead)
        {
            LockCursor();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (isDead)
        {
            return;
        }
        else if (GameIsPaused)
        {
            Play();
        }
        else
        {
            Stop();
        }
    }

    public void Play()
    {
        pauseMenuUI.SetActive(false);
        InGameUI.SetActive(true);
        if (settingsMenuUI != null)
        {
            settingsMenuUI.SetActive(false);
        }
        LockCursor();

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Stop()
    {
        pauseMenuUI.SetActive(true);
        InGameUI.SetActive(false);
        UnlockCursor();

        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void HandlePlayerDeath()
    {
        isDead = true;
        pauseMenuUI.SetActive(false);
        if (settingsMenuUI != null)
        {
            settingsMenuUI.SetActive(false);
        }
        UnlockCursor();

        Time.timeScale = 0f;
    }

    private void OnDestroy()
    {
        if (oxygenManager != null)
        {
            oxygenManager.OnPlayerDeath.RemoveListener(HandlePlayerDeath);
        }
    }

    private void LockCursor()
    {
#if UNITY_EDITOR
        AllowCursorLockInEditor();
#endif
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

#if UNITY_EDITOR
    private static System.Reflection.MethodInfo setAllowCursorLock;
    private static object[] allowCursorLockArgs;
    private static bool editorCursorLookupDone;

    private static void AllowCursorLockInEditor()
    {
        if (!editorCursorLookupDone)
        {
            editorCursorLookupDone = true;
            try
            {
                System.Reflection.BindingFlags internalStatic = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic;
                System.Type reasons = typeof(UnityEditor.Unsupported).GetNestedType("DisallowCursorLockReasons", System.Reflection.BindingFlags.NonPublic);
                setAllowCursorLock = typeof(UnityEditor.Unsupported).GetMethod("SetAllowCursorLock", internalStatic);
                if (reasons != null && setAllowCursorLock != null)
                {
                    allowCursorLockArgs = new object[] { true, System.Enum.Parse(reasons, "Other") };
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Couldn't re-allow cursor lock in the editor: " + e.Message);
            }
        }

        if (allowCursorLockArgs != null)
        {
            setAllowCursorLock.Invoke(null, allowCursorLockArgs);
        }
        UnityEditor.Unsupported.SetAllowCursorHide(true);
    }
#endif

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
