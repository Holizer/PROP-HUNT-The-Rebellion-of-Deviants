using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;

    [Header("Параметры камеры")]
    public float distance = 5.0f;
    public float sensitivityX = 150f;
    public float sensitivityY = 100f;
    public float smoothTime = 0.1f;
    public bool invertY = false;

    [Header("Лимиты вращения")]
    public float yMinLimit = 10f;
    public float yMaxLimit = 70f;

    [Header("Обработка коллизий")]
    public LayerMask collisionMask;
    public float minCameraDistance = 1.0f;
    public float collisionHeightOffset = 1.5f;
    public float cameraRadius = 0.3f;

    private float currentX = 0f;
    private float currentY = 0f;
    private Vector3 currentVelocity;

    private bool isAiming = false; // Флаг для отслеживания состояния прицеливания

    void Update()
    {
        if (!isAiming) // Обновляем только если не прицеливаемся
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;
            currentY += invertY ? mouseY : -mouseY;
            currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
        }
    }

    void LateUpdate()
    {
        if (!isAiming)
        {
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 desiredPosition = player.position + rotation * new Vector3(0, 0, -distance);

            Vector3 direction = desiredPosition - player.position;

            RaycastHit hit;

            if (Physics.SphereCast(player.position + Vector3.up * collisionHeightOffset, cameraRadius, direction.normalized, out hit, distance, collisionMask))
            {
                float adjustedDistance = Mathf.Clamp(hit.distance - cameraRadius, minCameraDistance, distance);
                desiredPosition = player.position + direction.normalized * adjustedDistance;
                desiredPosition.y = player.position.y + collisionHeightOffset;
            }

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
            transform.LookAt(player.position + Vector3.up * 1.5f);
        }
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }
}