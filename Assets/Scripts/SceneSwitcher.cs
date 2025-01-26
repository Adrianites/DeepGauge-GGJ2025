using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Scene scene;
    public enum Scene
    {
        MainMenu,
        Level1,
        Level2,
        Level3,
        End
    }

    public void SwitchScene()
    {
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
    }
}
