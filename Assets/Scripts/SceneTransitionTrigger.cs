using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string sceneToLoad = "FirstLevel"; 
    public string spawnPointID; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
}