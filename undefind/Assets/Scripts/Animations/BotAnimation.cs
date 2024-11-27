using UnityEngine;

public class BotAnimation : MonoBehaviour
{
    [Header("Компоненты")]
    public Animator animator;

    [Header("Переменные скорости")]
    float velocity = 0.0f;

    [Header("Настройки движения")]
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float deceleration = 1.8f;
    [SerializeField] private float runMultiplier = 4.0f;
    [SerializeField] private float quickStopMultiplier = 4.0f;

    [Header("Хеши анимаций")]
    int VelocityHash;

    void Start()
    {
        VelocityHash = Animator.StringToHash("Velocity");
    }

    void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat(VelocityHash, velocity);
    }
}
