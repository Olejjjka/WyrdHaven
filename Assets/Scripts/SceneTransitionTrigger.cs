using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    public string sceneToLoad = "FirstLevel"; // �������� ����� ��� ��������

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered by: " + other.name); // ��������� ��� �������
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone."); // ��������� ��� �������
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        Debug.Log("Loading scene: " + sceneToLoad); // ��������� ��� �������
        PlayerPrefs.SetString("SceneToLoad", sceneToLoad);
        SceneManager.LoadScene("LoadingScene");
    }
}
