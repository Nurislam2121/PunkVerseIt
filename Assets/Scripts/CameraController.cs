using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ссылка на игрока
    public Vector3 offset = new Vector3(0, 5, -10); // Смещение камеры относительно игрока
    public float smoothSpeed = 0.125f; // Скорость сглаживания движения камеры
    public float rotationSpeed = 5f; // Скорость сглаживания поворота камеры

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Желаемая позиция камеры
        Vector3 desiredPosition = target.position + offset;

        // Плавное перемещение камеры
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

        // Плавный поворот камеры к игроку
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
