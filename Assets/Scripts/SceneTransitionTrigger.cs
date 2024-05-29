using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string sceneToLoad = "FirstLevel"; // Название сцены для загрузки

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.instance.SavePlayerState(); // Сохранение состояния игрока перед выходом из сцены
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        PlayerPrefs.SetString("SceneToLoad", sceneToLoad);
        SceneManager.LoadScene("LoadingScene");
    }
}
