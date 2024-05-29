using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // ������ �� ��������� PlayerManager
    public GameObject playerPrefab; // ������ ������
    public GameObject player; // ������ �� ������ (Player)
    private Vector3 playerPosition;
    private Quaternion playerRotation;

    void Awake()
    {
        // ��������, ��� ���� ������ ���� ��������� PlayerManager � �����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ������ ��� �������� ����� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ���������� ��������� ������
    public void SavePlayerState()
    {
        if (player != null)
        {
            playerPosition = player.transform.position;
            playerRotation = player.transform.rotation;
        }
    }

    // �������� ��������� ������
    public void LoadPlayerState()
    {
        if (player != null)
        {
            player.transform.position = playerPosition;
            player.transform.rotation = playerRotation;
        }
    }

    // ���������� ������ �� ������ ��� �������� ����� �����
    [System.Obsolete]
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���� ����� ��������� �� ��������������
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        string spawnPointID = PlayerPrefs.GetString("SpawnPointID");
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.spawnPointID == spawnPointID)
            {
                if (player == null)
                {
                    // ������� ������, ���� ��� ��� ���
                    player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                }
                else
                {
                    // ���������� ������������� ������
                    player.transform.localPosition = spawnPoint.transform.position;
                    player.transform.localRotation = spawnPoint.transform.rotation;
                }
                break;
            }
        }

        DontDestroyOnLoad(player); // �� ���������� ������ ��� �������� ����� ����

        // �������� ������ �� ������ � ������
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.UpdateTarget(); // ���������� ������ �� ������ � ������
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
