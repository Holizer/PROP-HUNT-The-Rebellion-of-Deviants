using Photon.Pun;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class HiderMovement : MonoBehaviour
{
    [Header("Компоненты")]
    public Transform player;
    public Transform cameraTransform;

    [SerializeField] private CharacterController controller;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    [SerializeField] private PhotonView view;

    [Header("Настройки движения")]
    public float speed = 2f;
    public float runSpeedMultiplier = 2.5f;
    public float accelerationTime = 0.2f;
    public float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    private float currentSpeed;
    private float targetSpeed;
    private float speedVelocity;

    [Header("Параметры падения и гравитации")]
    public float gravity = -9.8f;
    public float fallSpeed = 0f;
    public float terminalVelocity = -53f;
    void Start()
    {
        if (controller == null)
        {
            controller = player.GetComponent<CharacterController>();
        }

        thirdPersonCamera = cameraTransform.GetComponent<ThirdPersonCamera>();
        if (thirdPersonCamera == null)
        {
            Debug.LogError($"На объекте {cameraTransform.name} отсутствует компонент ThirdPersonCamera.");
            return;
        }
        
        view = player.GetComponent<PhotonView>();
        if (view == null)
        {
            Debug.LogError("PhotonView не найден в дочерних объектах Hider.");
            return;
        }

        currentSpeed = speed;
        targetSpeed = speed;
    }

    void Update()
    {
        if (view.IsMine)
        {
            if (!(thirdPersonCamera.currentState is NormalCameraState && NormalCameraState.isReturningToNormal))
            {
                HandleMovement();
            }
            HandleGravity();
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
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(player.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            player.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        }
    }

}
