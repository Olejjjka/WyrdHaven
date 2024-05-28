using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    void Start()
    {
        GameIsPaused = false; // Сброс переменной при старте сцены
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
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Восстановите нормальную скорость времени перед загрузкой главного меню
        GameIsPaused = false; // Сброс переменной перед загрузкой главного меню
        LoadScene("MainMenu");
    }

    void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("SceneToLoad", sceneName);
        SceneManager.LoadScene("LoadingScene"); // Замените "LoadingScene" на имя вашей сцены загрузки
    }
}
