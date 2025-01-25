using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction escapeKey;

    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject InGameUI;

    void Start()
    {   
        pauseMenuUI.SetActive(false);
        InGameUI.SetActive(true);

        Time.timeScale = 1f;
    }

    public void Pause()
    {
        if (GameIsPaused)
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
}
