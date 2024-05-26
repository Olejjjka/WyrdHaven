using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar; // Ссылка на слайдер
    public Image progressIcon; // Ссылка на иконку

    void Start()
    {
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        // Асинхронная загрузка сцены
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!operation.isDone)
        {
            // Вычисление прогресса загрузки
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Обновление значения слайдера
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            // Обновление позиции иконки
            if (progressIcon != null)
            {
                RectTransform iconRectTransform = progressIcon.rectTransform;
                RectTransform fillRectTransform = progressBar.fillRect;

                // Вычисление новой позиции иконки в пределах fillRectTransform
                float fillWidth = fillRectTransform.rect.width;
                float newX = progress * fillWidth - fillWidth / 2f;
                iconRectTransform.anchoredPosition = new Vector2(newX, iconRectTransform.anchoredPosition.y);
            }

            yield return null;
        }
    }
}
