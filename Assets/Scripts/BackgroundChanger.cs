using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundChanger : MonoBehaviour
{
    public Sprite[] backgrounds;
    public Image backgroundImage;
    public Button nextButton;
    public string sceneToLoad = "BossLocation";
    public string spawnPointID;

    private int currentIndex = 0;

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
            PlayerManager.instance.SavePlayerState();
            PlayerPrefs.SetString("SpawnPointID", spawnPointID);
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
