using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAnimation : MonoBehaviour
{
    [Header("Аниматор")]
    public Animator animator;
    
    [Header("Камера")]
    public Transform cameraTransform;
    [SerializeField] private ThirdPersonCamera thirdPersonCamera;
    
    private PhotonView view;

    [Header("Переменные скорости")]
    private float velocity = 0.0f;
    private float pistolVelocity = 0.0f;

    [Header("Настройки движения")]
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float deceleration = 0.6f;
    [SerializeField] private float runMultiplier = 4.0f;
    [SerializeField] private float quickStopMultiplier = 4.0f;
    [SerializeField] private float pistolAcceleration = 0.8f;
    [SerializeField] private float pistolDeceleration = 1.0f;

    [Header("Хеши анимаций")]
    private int VelocityHash;
    private int PistolVelocityHash;
    private int IsAimingHash;

    void Start()
    {
        view = GetComponent<PhotonView>();
        
        if(thirdPersonCamera == null)
        {
            thirdPersonCamera = cameraTransform.GetComponent<ThirdPersonCamera>();
        }

        VelocityHash = Animator.StringToHash("Velocity");
        PistolVelocityHash = Animator.StringToHash("PistolVelocity");
        IsAimingHash = Animator.StringToHash("isAiming");
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

        bool forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool aimPressed = Input.GetMouseButton(1);

        if (animator != null)
        {
            animator.SetBool("isAiming", aimPressed);
        }

        if (aimPressed)
        {
            HandleAiming();
        }
        else
        {
            if (thirdPersonCamera.currentState is NormalCameraState && NormalCameraState.isReturningToNormal)
            {
                velocity = 0.0f;
            }
            else
            {
                HandleMovement(forwardPressed, runPressed);
            }
        }

        if (view.IsMine)
        {
            SyncAnimationParameters(aimPressed);
        }

        UpdateAnimatorParameters(aimPressed);
    }

    private void HandleAiming()
    {
        velocity = 0.0f;
        // Переход в анимацию прицеливания
        if (pistolVelocity > 0.0f)
        {
            pistolVelocity -= Time.deltaTime * pistolDeceleration;
        }
        // Возвращение из анимации прицеливания
        else if (pistolVelocity < 1.0f)
        {
            pistolVelocity += Time.deltaTime * pistolAcceleration;
        }
        
        pistolVelocity = Mathf.Max(pistolVelocity, 0.0f);
    }

    private void HandleMovement(bool forwardPressed, bool runPressed)
    {
        // Ускорение
        if (runPressed && forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration * runMultiplier;
        }

        // Обычное движение
        else if (forwardPressed && velocity < 0.2f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        // Переход в обччное движение после ускорение
        else if (forwardPressed && !runPressed && velocity > 0.2f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        // Остановка
        else if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        // Быстрая остановка после бега, полное отжатие кнопок
        else if (!runPressed && velocity > 0.2f && !forwardPressed)
        {
            velocity -= Time.deltaTime * deceleration * quickStopMultiplier;
        }

        velocity = Mathf.Max(velocity, 0.0f);
    }

    private void SyncAnimationParameters(bool aimPressed)
    {
        view.RPC("SyncHunterAnimationParameters", RpcTarget.Others, velocity, pistolVelocity, aimPressed);
    }

    private void UpdateAnimatorParameters(bool aimPressed)
    {
        animator.SetFloat(VelocityHash, velocity);
        animator.SetFloat(PistolVelocityHash, pistolVelocity);
        animator.SetBool(IsAimingHash, aimPressed);
    }
    private bool GameIsPaused()
    {
        return GetComponent<PlayerStateManager>().CurrentState is PauseState;
    }

    [PunRPC]
    void SyncHunterAnimationParameters(float velocity, float pistolVelocity, bool aimPressed)
    {
        this.velocity = velocity;
        this.pistolVelocity = pistolVelocity;
        UpdateAnimatorParameters(aimPressed);
    }
}