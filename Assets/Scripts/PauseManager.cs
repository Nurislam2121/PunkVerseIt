using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    [Header("Pause UI")]
    public GameObject pausePanel;

    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Скрываем панель при старте игры
        if (pausePanel != null)
            pausePanel.SetActive(false);

        ResumeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // 🔥 Пауза игры
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // стопируем игру
        pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ▶ Продолжить
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 🔁 Рестарт уровня
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🏠 Главное меню
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
        // Не забудь изменить название под свою сцену!
    }
}
