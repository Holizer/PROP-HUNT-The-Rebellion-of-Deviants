using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class HunterMovement : MonoBehaviour
{
    [Header("Компоненты")]
    public CharacterController controller;
    public Transform cameraTransform;
    [SerializeField] private PhotonView view;

    public bool isCameraTransition = false;

    [Header("Настройки движения")]
    public float speed = 3f;

    [Tooltip("Реальное ускорение игркока")]
    public float runSpeedMultiplier = 1.5f;

    [Tooltip("Ускорение анимации игрока при зажатии SHIFT")]
    public float accelerationTime = 0.2f;

    [Tooltip("Время поворота игрока")]
    public float turnSmoothTime = 0.1f;

    [SerializeField] private Animator animator;
    [SerializeField] private float turnSmoothVelocity;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float targetSpeed;
    [SerializeField] private float speedVelocity;

    void Start()
    {
        view = GetComponent<PhotonView>();
        currentSpeed = speed;
        targetSpeed = speed;
    }

    void Update()
    {
        if (view.IsMine && !isCameraTransition)
        {
            HandleMovement();
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

            if (moveDirection != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
            }
        }
    }
}
