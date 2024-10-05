using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.UI.Image;

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
    private Vector3 collisionVelocity; // Отдельная переменная для плавного движения при столкновении

    private BaseCameraState currentState;

    void Start()
    {
        currentState = new NormalCameraState(this);
    }

    public void SetState(BaseCameraState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
        else
        {
            SetState(new NormalCameraState(this));
        }
    }

    void LateUpdate()
    {
        if (currentState != null)
        {
            currentState.LateUpdateState();
        }
        else
        {
            SetState(new NormalCameraState(this));
        }
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

    public void HandleCollision(ref Vector3 desiredPosition)
    {
        Vector3 direction = desiredPosition - player.position;
        RaycastHit hit;

        Vector3 currentPosition = transform.position;
        Vector3 start = player.position + Vector3.up * collisionHeightOffset;

        if (Physics.SphereCast(start, cameraRadius, direction.normalized, out hit, distance, collisionMask))
        {
            float adjustedDistance = Mathf.Clamp(hit.distance - cameraRadius, minCameraDistance, distance);
            Vector3 adjustedPosition = player.position + direction.normalized * adjustedDistance;
            adjustedPosition.y = start.y;

            desiredPosition = Vector3.SmoothDamp(currentPosition, adjustedPosition, ref collisionVelocity, smoothTime);
        }
        else
        {
            desiredPosition = Vector3.SmoothDamp(currentPosition, desiredPosition, ref collisionVelocity, smoothTime);
        }
    }

    public Vector3 AimingRay(float maxAimDistance = 50f)
    {
        Vector3 direction = transform.forward;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        // Отладка луча
        Debug.DrawRay(transform.position, direction * maxAimDistance, Color.green);

        int playerLayer = LayerMask.GetMask("Player");
        if (Physics.Raycast(ray, out hit, maxAimDistance, ~playerLayer))
        {
            return hit.point;
        }

        return transform.position + direction * maxAimDistance;
    }
}
