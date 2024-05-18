using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // —сылка на игрока, за которым будет следовать камера
    public float smoothSpeed = 0.125f; // —корость перемещени€ камеры

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + new Vector3(0, 0, -10); // ѕолучаем желаемую позицию дл€ камеры
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // ѕлавно перемещаем камеру к желаемой позиции
            transform.position = smoothedPosition;
        }
    }
}
