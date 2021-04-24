using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void OnPlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitGame()
    {
        Application.Quit(0);
    }
}
