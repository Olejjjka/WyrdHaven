using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance; // ������ �� ��������� PlayerManager
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
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = GameObject.FindWithTag("Player"); // ����� ������ ������ �� ����
        LoadPlayerState();
        DontDestroyOnLoad(player); // �� ���������� ������ ��� �������� ����� ����
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
