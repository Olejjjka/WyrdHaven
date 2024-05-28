using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    void Start()
    {
        GameIsPaused = false; // ����� ���������� ��� ������ �����
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
        Time.timeScale = 1f; // ������������ ���������� �������� ������� ����� ��������� �������� ����
        GameIsPaused = false; // ����� ���������� ����� ��������� �������� ����
        LoadScene("MainMenu");
    }

    void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("SceneToLoad", sceneName);
        SceneManager.LoadScene("LoadingScene"); // �������� "LoadingScene" �� ��� ����� ����� ��������
    }
}
