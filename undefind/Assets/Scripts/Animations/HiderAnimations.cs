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
    [SerializeField] private float deceleration = 0.8f;
    [SerializeField] private float runMultiplier = 3.0f;
    [SerializeField] private float quickStopMultiplier = 4.0f;
    
    [Header("Хеши анимаций")]
    int VelocityHash;

    void Start()
    {
        view = GetComponentInParent<PhotonView>();

        Transform model = transform.Find("Model");
        if (model == null)
        {
            Debug.LogError("Model не найден внутри HiderPrefab!");
            return;
        }

        animator = model.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator не найден внутри Model или его дочерних объектов!");
            return;
        }

        PhotonAnimatorView photonAnimatorView = model.GetComponentInChildren<PhotonAnimatorView>();
        photonAnimatorView.enabled = true;

        VelocityHash = Animator.StringToHash("Velocity");
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

        if (view.IsMine)
        {
            view.RPC("SyncHiderAnimationParameters", RpcTarget.Others, velocity);
        }
        animator.SetFloat(VelocityHash, velocity);
    }

    [PunRPC]
    void SyncHiderAnimationParameters(float velocity)
    {
        animator.SetFloat(VelocityHash, velocity);
    }
}