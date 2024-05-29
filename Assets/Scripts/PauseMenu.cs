using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; // ������ �� UI ������� ��������� ����

    void Start()
    {
        GameIsPaused = false; // ����� ���������� ��� ������ �����
        pauseMenuUI.SetActive(false); // �������, ��� ���� ����� ��������� � ������
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
        Time.timeScale = 1f; // ������������ ���������� �������� ������� ����� ��������� �������� ����
        GameIsPaused = false; // ����� ���������� ����� ��������� �������� ����
        PlayerManager.instance.DestroyPlayer(); // ������� �������� ������
        SceneManager.LoadScene("MainMenu");
    }
}

