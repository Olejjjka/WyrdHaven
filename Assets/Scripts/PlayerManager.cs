using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // Ссылка на экземпляр PlayerManager
    public GameObject player; // Ссылка на игрока (Player)
    private Vector3 playerPosition;
    private Quaternion playerRotation;

    void Awake()
    {
        // Убедимся, что есть только один экземпляр PlayerManager в сцене
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожаем объект при загрузке новых сцен
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Сохранение состояния игрока
    public void SavePlayerState()
    {
        if (player != null)
        {
            playerPosition = player.transform.position;
            playerRotation = player.transform.rotation;
        }
    }

    // Загрузка состояния игрока
    public void LoadPlayerState()
    {
        if (player != null)
        {
            player.transform.position = playerPosition;
            player.transform.rotation = playerRotation;
        }
    }

    // Обновление ссылки на игрока при загрузке новой сцены
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player"); // Найти объект игрока по тегу
        LoadPlayerState();
        DontDestroyOnLoad(player); // Не уничтожать игрока при загрузке новых сцен
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
