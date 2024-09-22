using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiderAnimations : MonoBehaviour
{
    [Header("Компоненты")]
    Animator animator;
    
    [Header("Переменные скорости")]
    float velocity = 0.0f;

    [Header("Настройки движения")]
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float deceleration = 0.8f;
    [SerializeField] private float runMultiplier = 3.0f;
    [SerializeField] private float quickStopMultiplier = 4.0f;
    
    [Header("Хеши анимаций")]
    int VelocityHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if (runPressed && forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration * runMultiplier;
        }

        if (forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (forwardPressed && !runPressed && velocity > .5f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (!runPressed && velocity > 0.5f && !forwardPressed)
        {
            velocity -= Time.deltaTime * deceleration * quickStopMultiplier;
        }

        if (velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat(VelocityHash, velocity);
    }
}