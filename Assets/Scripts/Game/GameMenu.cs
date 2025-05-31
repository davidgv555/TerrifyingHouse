using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("DemoFinal");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
