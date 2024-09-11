using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationScriptController : MonoBehaviour
{
    Animator animator;
    float velocity = 0.0f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.8f;
    [SerializeField] private float runMultiplier = 6.0f;
    [SerializeField] private float quickStopMultiplier = 4.0f;
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

        if (forwardPressed && !runPressed && velocity > .2f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (!runPressed && velocity > 0.2f && !forwardPressed)
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
