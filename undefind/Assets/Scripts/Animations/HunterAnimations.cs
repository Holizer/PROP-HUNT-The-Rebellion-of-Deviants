using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterAnimation : MonoBehaviour
{
    [Header("����������")]
    public Animator animator;
    [SerializeField] private PhotonView view;

    [Header("���������� ��������")]
    private float velocity = 0.0f;
    private float pistolVelocity = 0.0f;

    [Header("��������� ��������")]
    [SerializeField] private float acceleration = 0.3f;             
    [SerializeField] private float deceleration = 0.6f;        
    [SerializeField] private float runMultiplier = 4.0f;           
    [SerializeField] private float quickStopMultiplier = 4.0f;
    [SerializeField] private float pistolAcceleration = 0.8f;
    [SerializeField] private float pistolDeceleration = 1.0f;

    [Header("���� ��������")]
    private int VelocityHash;
    private int PistolVelocityHash;
    private int IsAimingHash;

    void Start()
    {
        view = GetComponentInParent<PhotonView>();
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
        bool forwardPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool aimPressed = Input.GetMouseButton(1);

        if (animator != null)
        {
            animator.SetBool("isAiming", aimPressed);
        }

        if (aimPressed)
        {
            velocity = 0.0f;
            if (pistolVelocity < 1.0f)
            {
                pistolVelocity += Time.deltaTime * pistolAcceleration;
            }
        }
        else
        {
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

            if (pistolVelocity > 0.0f)
            {
                pistolVelocity -= Time.deltaTime * pistolDeceleration;
            }

            if (velocity < 0.0f)
            {
                velocity = 0.0f;
            }

            if (pistolVelocity < 0.0f)
            {
                pistolVelocity = 0.0f;
            }
        }

        if (view.IsMine)
        {
            view.RPC("SyncHunterAnimationParameters", RpcTarget.Others, velocity, pistolVelocity, aimPressed);
        }

        animator.SetFloat(VelocityHash, velocity);
        animator.SetFloat(PistolVelocityHash, pistolVelocity);
        animator.SetBool(IsAimingHash, aimPressed);
    }

    [PunRPC]
    void SyncHunterAnimationParameters(float velocity, float pistolVelocity, bool isAiming)
    {
        animator.SetFloat(VelocityHash, velocity);
        animator.SetFloat(PistolVelocityHash, pistolVelocity);
        animator.SetBool(IsAimingHash, isAiming);
    }
}
