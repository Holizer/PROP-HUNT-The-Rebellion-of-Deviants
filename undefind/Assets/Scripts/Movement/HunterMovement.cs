using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class HunterMovement : MonoBehaviourPun, IPunObservable
{
    [Header("Компоненты")]
    public Transform player;
    [SerializeField] private CharacterController controller;

    [Header("Камера")]
    public Transform cameraTransform;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;

    [Header("Игровая модель")]
    public Transform playerModel;

    private PhotonView view;

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

    private float targetAngle; // Добавлено для плавного угла поворота

    void Start()
    {
        view = player.GetComponent<PhotonView>();

        if (controller == null)
        {
            controller = player.GetComponent<CharacterController>();
        }

        if (thirdPersonCamera == null)
        {
            thirdPersonCamera = cameraTransform.GetComponent<ThirdPersonCamera>();
        }

        currentSpeed = speed;
        targetSpeed = speed;
        targetAngle = playerModel.eulerAngles.y;
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
        else
        {
            float smoothAngle = Mathf.SmoothDampAngle(playerModel.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            playerModel.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
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
        Vector3 aimDirection = aimTargetPoint - playerModel.transform.position;
        aimDirection.y = 0;

        HandleRotation(aimDirection);
    }

    void HandleRotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            float newTargetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(playerModel.eulerAngles.y, newTargetAngle, ref turnSmoothVelocity, turnSmoothTime);

            playerModel.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            targetAngle = newTargetAngle;
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            float angle = playerModel.transform.rotation.eulerAngles.y;
            //Debug.Log($"Sending rotation angle: {angle}");
            stream.SendNext(angle);
        }
        else
        {
            // Получаем поворот с другого клиента
            if (stream.Count > 0)
            {
                object received = stream.ReceiveNext();
                if (received is float receivedAngle)
                {
                    //Debug.Log($"Received rotation angle: {receivedAngle}");
                    targetAngle = receivedAngle;
                }
                else
                {
                    //Debug.LogError("Received data is not a float.");
                }
            }
        }
    }
}
