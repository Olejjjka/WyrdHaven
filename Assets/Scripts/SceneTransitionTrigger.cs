using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string sceneToLoad = "FirstLevel"; // Название сцены для загрузки

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name); // Добавлено для отладки
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone."); // Добавлено для отладки
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading scene: " + sceneToLoad); // Добавлено для отладки
        PlayerPrefs.SetString("SceneToLoad", sceneToLoad);
        SceneManager.LoadScene("LoadingScene");
    }
}
