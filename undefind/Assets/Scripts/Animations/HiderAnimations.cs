using Photon.Pun;
using UnityEngine;

public class HiderAnimation : MonoBehaviour
{
    [Header("Компоненты")]
    [SerializeField] private Animator animator;
    [SerializeField] private PhotonView view;

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
        Transform model = transform.Find("Model");
        if (model == null)
        {
            Debug.LogError("Не найден контейнер Model!");
            return;
        }

        animator = model.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator не найден внутри Model или его дочерних объектов!");
            return;
        }

        view = transform.GetComponent<PhotonView>();
        if (view == null)
        {
            Debug.LogError("PhotonView не найден на объекте!");
            return;
        }

        VelocityHash = Animator.StringToHash("Velocity");

        PhotonAnimatorView photonAnimatorView = model.GetComponentInChildren<PhotonAnimatorView>();
        if (photonAnimatorView == null)
        {
            Debug.LogError("PhotonAnimatorView не найден на объекте!");
            return;
        }
        photonAnimatorView.enabled = true;
    }

    void Update()
    {
        if (view.IsMine)
        {
            UpdateAniamtion();
        }
    }

    private void UpdateAniamtion()
    {
        if (GameIsPaused())
        {
            velocity -= Time.deltaTime * deceleration;
            animator.SetFloat(VelocityHash, velocity);
            return;
        }

        // Обрабатываем ввод клавиш для передвижения
        bool forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        // Если игрок бежит и нажаты клавиши движения
        if (runPressed && forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration * runMultiplier; 
        }

        // Если просто идёт
        if (forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration; 
        }

        // Если движется, но перестаёт бежать
        if (forwardPressed && !runPressed && velocity > .2f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        // Если не двигается, замедляемся до остановки
        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        // Если резко останавливается, применяем дополнительное замедление
        if (!runPressed && velocity > 0.2f && !forwardPressed)
        {
            velocity -= Time.deltaTime * deceleration * quickStopMultiplier;
        }

        // Ограничиваем скорость ниже нуля
        if (velocity < 0.0f)
        {
            velocity = 0.0f;
        }

        animator.SetFloat(VelocityHash, velocity);
    }

    private bool GameIsPaused()
    {
        return GetComponent<PlayerStateManager>().CurrentState is PauseState;
    }
}