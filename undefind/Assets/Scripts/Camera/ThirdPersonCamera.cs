using UnityEditor.Rendering;
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

    private ICameraState currentState;

    void Start()
    {
        SetState(new NormalCameraState(this));
    }
    public void SetState(ICameraState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    // NormalCameraState и AimingCameraState установлены обновления состояний камреы
    void Update()
    {
        currentState.UpdateState();
    }

    void LateUpdate()
    {
        currentState.LateUpdateState();
    }
    public void UpdateRotation(float deltaX, float deltaY)
    {
        currentX += deltaX * sensitivityX * Time.deltaTime;
        currentY += invertY ? deltaY : -deltaY;
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(currentY, currentX, 0);
    }

    public void SmoothPosition(Vector3 desiredPosition)
    {
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
    }

    public void LookAt(Vector3 target)
    {
        transform.LookAt(target);
    }
    public void HandleCollision(Vector3 desiredPosition)
    {
        Vector3 direction = desiredPosition - player.position;
        RaycastHit hit;
        if (Physics.SphereCast(player.position + Vector3.up * collisionHeightOffset, cameraRadius, direction.normalized, out hit, distance, collisionMask))
        {
            float adjustedDistance = Mathf.Clamp(hit.distance - cameraRadius, minCameraDistance, distance);
            desiredPosition = player.position + direction.normalized * adjustedDistance;
            desiredPosition.y = player.position.y + collisionHeightOffset;
        }

        SmoothPosition(desiredPosition);
    }
}
