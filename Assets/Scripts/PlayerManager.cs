using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // Ссылка на экземпляр PlayerManager
    public GameObject playerPrefab; // Префаб игрока
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

    public void SavePlayerState()
    {
        if (player != null)
        {
            playerPosition = player.transform.position;
            playerRotation = player.transform.rotation;
        }
    }

    public void LoadPlayerState()
    {
        if (player != null)
        {
            player.transform.position = playerPosition;
            player.transform.rotation = playerRotation;
        }
    }

    public void DestroyPlayer()
    {
        if (player != null)
        {
            Destroy(player);
            player = null;
        }
    }

    [System.Obsolete]
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ищем точку появления по идентификатору
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        string spawnPointID = PlayerPrefs.GetString("SpawnPointID");
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnPointID == spawnPointID)
            {
                if (player == null)
                {
                    // Создаем игрока, если его еще нет
                    player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                }
                else
                {
                    // Перемещаем существующего игрока
                    player.transform.position = spawnPoint.transform.position;
                    player.transform.rotation = spawnPoint.transform.rotation;
                }
                break;
            }
        }

        if (player != null)
        {
            DontDestroyOnLoad(player); // Не уничтожаем игрока при загрузке новых сцен
        }

        // Обновить ссылку на игрока в камере
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.UpdateTarget(); // Обновление ссылки на игрока в камере
        }
    }

    [System.Obsolete]
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    [System.Obsolete]
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
