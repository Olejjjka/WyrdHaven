using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; // Ссылка на UI элемент паузового меню

    void Start()
    {
        GameIsPaused = false; // Сброс переменной при старте сцены
        pauseMenuUI.SetActive(false); // Убедись, что меню паузы отключено в начале
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Восстановите нормальную скорость времени перед загрузкой главного меню
        GameIsPaused = false; // Сброс переменной перед загрузкой главного меню
        PlayerManager.instance.DestroyPlayer(); // Удаляем текущего игрока
        SceneManager.LoadScene("MainMenu");
    }
}

