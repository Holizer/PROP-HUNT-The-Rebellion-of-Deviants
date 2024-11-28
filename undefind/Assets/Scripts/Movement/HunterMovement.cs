using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class HunterMovement : MonoBehaviourPun
{
    [Header("Компоненты")]
    public CharacterController controller;
    public Transform cameraTransform;
    private PhotonView view;
    private ThirdPersonCamera thirdPersonCamera;

    [Header("Настройки движения")]
    public float speed = 2f;
    public float runSpeedMultiplier = 2.5f;
    public float accelerationTime = 0.2f;
    public float turnSmoothTime = 0.1f;

    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float targetSpeed;
    [SerializeField] private float speedVelocity;

    [Header("Параметры падения и гравитации")]
    public float gravity = -9.8f;
    public float fallSpeed = 0f;
    public float terminalVelocity = -53f;
    void Start()
    {
        view = GetComponent<PhotonView>();
        thirdPersonCamera = cameraTransform.GetComponent<ThirdPersonCamera>();
        currentSpeed = speed;
        targetSpeed = speed;
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (thirdPersonCamera.currentState is AimingCameraState)
            {
                HandleAimingRotation();
            }
            else if (!(thirdPersonCamera.currentState is NormalCameraState && NormalCameraState.isReturningToNormal))
            {
                HandleMovement();
            }

            HandleGravity();
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool runPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3");

        targetSpeed = runPressed ? speed * runSpeedMultiplier : speed;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, accelerationTime);

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0f;
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            HandleRotation(moveDirection);
            controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
        }
    }

    void HandleAimingRotation()
    {
        Vector3 aimTargetPoint = thirdPersonCamera.CalculateAimPoint();
        Vector3 aimDirection = aimTargetPoint - transform.position;
        aimDirection.y = 0;

        HandleRotation(aimDirection);
    }

    void HandleRotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        }
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
        {
            fallSpeed = -2f;
        }
        else
        {
            fallSpeed += gravity * Time.deltaTime;
        }

        if (fallSpeed < terminalVelocity)
        {
            fallSpeed = terminalVelocity;
        }

        Vector3 gravityMove = new Vector3(0f, fallSpeed, 0f);
        controller.Move(gravityMove * Time.deltaTime);
    }
}
