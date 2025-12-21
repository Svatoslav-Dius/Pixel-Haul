using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Вихід з гри!");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Menu");
    }
}