using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    [Header("Компоненты")]
    public CharacterController controller;

    [Header("Настройки движения")]
    public float speed = 2f;
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

    [Header("Цель для движения")]
    public Transform target;
    private Vector3 direction;

    void Start()
    {
        currentSpeed = speed;
        targetSpeed = speed;
    }

    void Update()
    {
        if (target != null)
        {
            HandleMovement();
            HandleGravity();
        }
        else
        {
            Wander();
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
        direction = (target.position - transform.position).normalized;

        // No running logic here, always use walking speed
        targetSpeed = speed;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, accelerationTime);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    void Wander()
    {
        // Random wandering if no target is set
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        direction = randomDirection;

        targetSpeed = speed; // Always walk, never run
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, accelerationTime);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
    }
}
