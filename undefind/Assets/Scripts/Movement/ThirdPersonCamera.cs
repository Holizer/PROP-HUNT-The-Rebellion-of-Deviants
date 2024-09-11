using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;
    public float distance = 5.0f;
    public float sensitivityX = 150f;
    public float sensitivityY = 100f;
    public float yMinLimit = 10f;
    public float yMaxLimit = 70f;
    public float smoothTime = 0.1f;

    public LayerMask collisionMask;
    public float minCameraDistance = 1.0f;
    public float collisionHeightOffset = 1.5f;

    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 currentVelocity;

    void Update()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = player.position + rotation * new Vector3(0, 0, -distance);

        RaycastHit hit;
        if (Physics.Linecast(player.position, desiredPosition, out hit, collisionMask))
        {
            // Если есть препятствие, уменьшаем расстояние камеры до точки столкновения
            float adjustedDistance = Mathf.Clamp(hit.distance, minCameraDistance, distance);

            // Перемещаем камеру к этой точке, но поднимаем камеру выше при столкновении
            Vector3 directionToCamera = (desiredPosition - player.position).normalized;
            desiredPosition = player.position + directionToCamera * adjustedDistance;

            // Добавляем вертикальное смещение для предотвращения падения камеры к ногам
            desiredPosition.y += collisionHeightOffset;
        }

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

        transform.LookAt(player.position + Vector3.up * 1.5f);
    }
}
