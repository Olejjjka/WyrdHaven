using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public SpriteRenderer mapRenderer; // ������ �� SpriteRenderer ������� �������

    private float camHalfHeight;
    private float camHalfWidth;
    private Vector2 minPosition;
    private Vector2 maxPosition;

    void Start()
    {
        // ����������� �������� ������
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = cam.aspect * camHalfHeight;

        // ����������� ������ �� ������ ������� �������
        Bounds bounds = mapRenderer.bounds;
        minPosition = new Vector2(bounds.min.x + camHalfWidth, bounds.min.y + camHalfHeight);
        maxPosition = new Vector2(bounds.max.x - camHalfWidth, bounds.max.y - camHalfHeight);

        // ����� ������ ��� ������ �����
        if (PlayerManager.instance != null)
        {
            target = PlayerManager.instance.player.transform;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + new Vector3(0, 0, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // ����������� ������� ������ � �������� ������ �����
            smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minPosition.x, maxPosition.x);
            smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minPosition.y, maxPosition.y);

            transform.position = smoothedPosition;
        }
    }

    public void UpdateTarget()
    {
        if (PlayerManager.instance != null)
        {
            target = PlayerManager.instance.player.transform;
        }
    }
}
