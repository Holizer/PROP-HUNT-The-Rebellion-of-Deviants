using Photon.Pun;
using UnityEngine;

public class HunterMovement : MonoBehaviour
{
    [Header("����������")]
    public CharacterController controller;
    public Transform cameraTransform;
    private Animator animator;
    PhotonView view;

    [Header("��������� ��������")]
    public float speed = 3f;
    public float runSpeedMultiplier = 1.5f;
    public float accelerationTime = 0.2f;
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    private float currentSpeed;
    private float targetSpeed;
    private float speedVelocity;

    void Start()
    {
        view = GetComponent<PhotonView>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        currentSpeed = speed;
        targetSpeed = speed;
    }

    void Update()
    {
        if (view.IsMine)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool runPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3");

        bool aimingPressed = Input.GetMouseButton(1);
        if (animator != null)
        {
            animator.SetBool("isAiming", aimingPressed);
        }

        targetSpeed = runPressed ? speed * runSpeedMultiplier : speed;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, accelerationTime);

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (aimingPressed)
        {
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            if (cameraForward != Vector3.zero)
            {
                float targetAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
        else if (inputDirection.magnitude >= 0.1f)
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
