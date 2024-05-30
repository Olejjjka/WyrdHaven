using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;

    private void Start()
    {
        deathPanel.SetActive(false);
    }

    public void ShowDeathPanel()
    {
        Time.timeScale = 0f;
        deathPanel.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        PlayerManager.instance.DestroyPlayer();
        SceneManager.LoadScene("MainMenu");
    }
}
