using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;
    public Image progressIcon;

    void Start()
    {
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (progressIcon != null)
            {
                RectTransform iconRectTransform = progressIcon.rectTransform;
                RectTransform fillRectTransform = progressBar.fillRect;

                float fillWidth = fillRectTransform.rect.width;
                float newX = progress * fillWidth - fillWidth / 2f;
                iconRectTransform.anchoredPosition = new Vector2(newX, iconRectTransform.anchoredPosition.y);
            }

            yield return null;
        }
    }
}
