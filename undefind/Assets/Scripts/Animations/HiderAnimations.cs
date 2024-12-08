using Photon.Pun;
using UnityEngine;

public class HiderAnimation : MonoBehaviour
{
    [Header("����������")]
    [SerializeField] private Animator animator;
    [SerializeField] private PhotonView view;

    [Header("���������� ��������")]
    float velocity = 0.0f;

    [Header("��������� ��������")]
    [SerializeField] private float acceleration = 0.3f;
    [SerializeField] private float deceleration = 1.8f;
    [SerializeField] private float runMultiplier = 4.0f;
    [SerializeField] private float quickStopMultiplier = 4.0f;
    
    [Header("���� ��������")]
    int VelocityHash;

    void Start()
    {
        Transform model = transform.Find("Model");
        if (model == null)
        {
            Debug.LogError("�� ������ ��������� Model!");
            return;
        }

        animator = model.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator �� ������ ������ Model ��� ��� �������� ��������!");
            return;
        }

        view = transform.GetComponent<PhotonView>();
        if (view == null)
        {
            Debug.LogError("PhotonView �� ������ �� �������!");
            return;
        }

        VelocityHash = Animator.StringToHash("Velocity");

        PhotonAnimatorView photonAnimatorView = model.GetComponentInChildren<PhotonAnimatorView>();
        if (photonAnimatorView == null)
        {
            Debug.LogError("PhotonAnimatorView �� ������ �� �������!");
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

        // ������������ ���� ������ ��� ������������
        bool forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        // ���� ����� ����� � ������ ������� ��������
        if (runPressed && forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration * runMultiplier; 
        }

        // ���� ������ ���
        if (forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration; 
        }

        // ���� ��������, �� �������� ������
        if (forwardPressed && !runPressed && velocity > .2f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        // ���� �� ���������, ����������� �� ���������
        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        // ���� ����� ���������������, ��������� �������������� ����������
        if (!runPressed && velocity > 0.2f && !forwardPressed)
        {
            velocity -= Time.deltaTime * deceleration * quickStopMultiplier;
        }

        // ������������ �������� ���� ����
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