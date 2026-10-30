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
        // Save current score
        int currentScore = ScoreManager.Instance != null ? ScoreManager.Instance.GetScore() : 0;

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

        // Load saved score
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.SetScore(currentScore);
        }
    }
}