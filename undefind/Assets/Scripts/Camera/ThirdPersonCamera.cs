using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;
using static UnityEngine.UI.Image;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;

    [Header("Параметры камеры")]
    public float distance = 5.0f;
    public float sensitivity = 150f;
    public float smoothTime = 0.1f;
    public bool invertY = false;

    [Header("Лимиты вращения")]
    public float yMinLimit = -20f;
    public float yMaxLimit = 20f;

    [Header("Обработка коллизий")]
    public LayerMask collisionMask;
    public float minCameraDistance = 1.0f;
    public float collisionHeightOffset = 1.5f;
    public float cameraRadius = 0.3f;

    public float currentX = 0f;
    public float currentY = 0f;
    private Vector3 currentVelocity;
    private Vector3 collisionVelocity;

    public BaseCameraState currentState { get; private set; }
    void Start()
    {
        SetState(new NormalCameraState(this));
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
        currentX += deltaX * sensitivity * Time.deltaTime;
        currentY += invertY ? deltaY : -deltaY;
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
    }
    public void UpdateRotation(float deltaX, float deltaY, float sensitivity, bool invertY, float yMinLimit, float yMaxLimit)
    {
        currentX += deltaX * sensitivity * Time.deltaTime;
        currentY += invertY ? deltaY : -deltaY;
        currentY = Mathf.Clamp(currentY, yMinLimit, yMaxLimit);
    }
    public void UpdateRotation(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 euler = lookRotation.eulerAngles;
        currentX = euler.y;
        currentY = euler.x;
    }
    public void SetRotation()
    {
        transform.rotation = Quaternion.Euler(currentY, currentX, 0);
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
    public Vector3 HandleCollision(Vector3 desiredPosition)
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

        return desiredPosition;
    }
    public Vector3 AimingRay(float maxAimDistance = 50f)
    {
        Vector3 direction = transform.forward;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction * maxAimDistance, Color.green);

        int aimTargetLayerMask = LayerMask.GetMask("AimTarget");
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, maxAimDistance, aimTargetLayerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = transform.position + direction * maxAimDistance;
        }

        return targetPoint;
    }
}
