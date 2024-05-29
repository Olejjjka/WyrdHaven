using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string sceneToLoad = "FirstLevel"; // Название сцены для загрузки
    public string spawnPointID; // Идентификатор точки появления для следующей сцены

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
}
