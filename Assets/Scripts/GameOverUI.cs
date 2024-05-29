using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Time.timeScale = 1f; // ¬осстановите нормальную скорость времени перед загрузкой главного меню
        PlayerManager.instance.DestroyPlayer(); // ”дал€ем текущего игрока
        SceneManager.LoadScene("MainMenu");
    }
}
