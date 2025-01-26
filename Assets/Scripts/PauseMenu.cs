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
    public OxygenManager oxygenManager; 
    public bool isDead = false;

    void Start()
    {   
        pauseMenuUI.SetActive(false);
        InGameUI.SetActive(true);

        Time.timeScale = 1f;

        if (oxygenManager != null)
        {
            oxygenManager.OnPlayerDeath.AddListener(HandlePlayerDeath);
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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Stop()
    {
        pauseMenuUI.SetActive(true);
        InGameUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void HandlePlayerDeath()
    {
        isDead = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        Time.timeScale = 0f;
    }
}