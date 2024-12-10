using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotAI : MonoBehaviour
{
    [Header("Компоненты")]
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Настройки движения")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float turnSmoothTime = 0.1f; 
    private float turnSmoothVelocity;
    private float velocity = 0.0f;

    [Header("Настройки анимации")]
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float deceleration = 1.8f;

    [Header("Параметры анимации")]
    private int velocityHash;

    [Header("Точки интереса")]
    public List<Transform> pointsOfInterest;

    [Header("Патрулирование")]
    [SerializeField] private float patrolRange = 10f;
    [SerializeField] private float idleTime = 2f; 
    private float idleTimer = 0f;
    private bool isIdle = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent не найден на объекте!");
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator не найден внутри Model или его дочерних объектов!");
            return;
        }

        velocityHash = Animator.StringToHash("Velocity");

        // Настройка NavMeshAgent
        agent.speed = walkSpeed;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;

        SetRandomDestination();
    }

    void Update()
    {
        if (isIdle)
        {
            IdleBehavior();
            return;
        }

        // Обновление анимации
        UpdateAnimation();

        // Проверка достижения цели
        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            StartIdle();
        }
    }

    private void UpdateAnimation()
    {
        float targetVelocity = agent.velocity.magnitude / walkSpeed;

        if (targetVelocity > velocity)
        {
            velocity += Time.deltaTime * acceleration;
        }
        else if (targetVelocity < velocity)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        velocity = Mathf.Clamp(velocity, 0f, 0.2f);
        animator.SetFloat(velocityHash, velocity);

        // Плавный поворот (если бот движется)
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomPoint = GetRandomPoint(transform.position, patrolRange);
        agent.SetDestination(randomPoint);
    }

    private Vector3 GetRandomPoint(Vector3 center, float range)
    {
        Vector3 randomPoint = center + new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, range, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }

    private void StartIdle()
    {
        isIdle = true;
        idleTimer = idleTime;
        agent.ResetPath();
        animator.SetFloat(velocityHash, 0f);
    }

    private void IdleBehavior()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0f)
        {
            isIdle = false;
            SetRandomDestination();
        }
    }
}
