using UnityEngine;
using UnityEngine.SceneManagement; // ОБОВ'ЯЗКОВО: Бібліотека для сцен

public class MenuController : MonoBehaviour
{
    // Функція для кнопки "Грати"
    public void StartGame()
    {
        SceneManager.LoadScene("Level1"); // Завантажує сцену з назвою "Game"
    }

    // Функція для кнопки "Вихід" (працює у зібраній грі)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Вихід з гри!");
    }

    // Функція для перезапуску (для сцени перемоги)
    public void RestartGame()
    {
        SceneManager.LoadScene("Menu"); // Повертає в меню
    }
}