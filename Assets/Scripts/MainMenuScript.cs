using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject btns;
    // Метод для кнопки "Start"
    public void StartGame()
    {
        shopPanel.SetActive(true);
        btns.SetActive(false);
        // Переход на сцену "Level1"
        //SceneManager.LoadScene("Level1");
    }

    // Метод для кнопки "Exit"
    public void ExitGame()
    {
        // Если игра запущена в редакторе — просто выводим сообщение
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Выход из игры
        Application.Quit();
#endif
    }
}
