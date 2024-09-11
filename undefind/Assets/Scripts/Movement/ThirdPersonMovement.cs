using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;

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
        currentSpeed = speed;
        targetSpeed = speed;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool runPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3");

        targetSpeed = runPressed ? speed * runSpeedMultiplier : speed;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, accelerationTime);

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }
    }
}
