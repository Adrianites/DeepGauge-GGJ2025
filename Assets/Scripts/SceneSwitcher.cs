using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Collider col;
    public Scene scene;
    public enum Scene
    {
        MainMenu,
        Level1,
        Level2,
        Level3,
        End
    }

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchScene();
        }
    }

    public void SwitchScene()
    {
        // Save current score and oxygen levels
        int currentScore = ScoreManager.Instance?.GetScore() ?? 0;
        float currentOxygen = OxygenManager.Instance?.GetCurrentOxygen() ?? 100f;

        // Switch scene
        switch (scene)
        {
            case Scene.MainMenu:
                SceneManager.LoadScene("MainMenu");
                break;
            case Scene.Level1:
                SceneManager.LoadScene("Level1");
                break;
            case Scene.Level2:
                SceneManager.LoadScene("Level2");
                break;
            case Scene.Level3:
                SceneManager.LoadScene("Level3");
                break;
            case Scene.End:
                SceneManager.LoadScene("End");
                break;
        }

        // Load saved score and oxygen levels
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScore(currentScore);
        }
        if (OxygenManager.Instance != null)
        {
            OxygenManager.Instance.SetCurrentOxygen(currentOxygen);
        }
    }
}