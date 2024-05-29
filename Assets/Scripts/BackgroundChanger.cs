using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundChanger : MonoBehaviour
{
    public Sprite[] backgrounds; // Массив для хранения спрайтов фона
    public Image backgroundImage; // Image компонент для отображения фона
    public Button nextButton; // Кнопка "Далее"
    public string sceneToLoad = "BossLocation"; // Название сцены для загрузки
    public string spawnPointID; // Идентификатор точки появления для следующей сцены

    private int currentIndex = 0; // Текущий индекс спрайта

    void Start()
    {
        nextButton.onClick.AddListener(OnNextButtonClick);
        UpdateBackground();
    }

    void OnNextButtonClick()
    {
        currentIndex++;
        if (currentIndex < backgrounds.Length)
        {
            UpdateBackground();
        }
        else
        {
            PlayerManager.instance.SavePlayerState(); // Сохранение состояния игрока перед выходом из сцены
            PlayerPrefs.SetString("SpawnPointID", spawnPointID); // Сохранение идентификатора точки появления
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        PlayerPrefs.SetString("SceneToLoad", sceneToLoad);
        SceneManager.LoadScene("LoadingScene");
    }

    void UpdateBackground()
    {
        backgroundImage.sprite = backgrounds[currentIndex];
    }
}
