using Unity.VisualScripting;
using UnityEngine;

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
    private Vector3 lastAimPoint;

    public BaseCameraState currentState { get; private set; }

    [Header("Fade эффект")]
    public ScreenFader screenFader;
    private void Awake()
    {
        if (screenFader != null)
        {
            screenFader.FadeIn();
        }
    }

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
        float sphereRadius = cameraRadius;

        RaycastHit hit;
        if (Physics.SphereCast(player.position + Vector3.up * collisionHeightOffset, sphereRadius, direction.normalized, out hit, direction.magnitude))
        {
            if (hit.collider != null)
            {
                float distanceToCollision = hit.distance - sphereRadius;
                desiredPosition = player.position + direction.normalized * Mathf.Max(distanceToCollision, minCameraDistance);
                desiredPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10f);
                desiredPosition.y = player.position.y + collisionHeightOffset;
            }
        }
        return desiredPosition;
    }



    public Vector3 CalculateAimPoint(float maxAimDistance = 50f)
    {
        Vector3 direction = transform.forward;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(transform.position, direction * maxAimDistance, Color.green);

        int aimTargetLayerMask = LayerMask.GetMask("AimTarget");
        if (Physics.Raycast(ray, out hit, maxAimDistance, aimTargetLayerMask))
        {
            lastAimPoint = hit.point;
        }
        else
        {
            lastAimPoint = transform.position + direction * maxAimDistance;
        }

        return lastAimPoint;
    }

    public Vector3 GetLastAimPoint()
    {
        return lastAimPoint;
    }
}