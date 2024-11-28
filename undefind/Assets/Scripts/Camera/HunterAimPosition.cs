using Photon.Realtime;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HunterAimPosition : MonoBehaviour
{
    [Header("Компоненты")]
    public ThirdPersonCamera thirdPersonCamera;

    public Rig animationRig;
    public RigController rigController;
    public RigBuilder rigBuilder;
    public GameObject aimTarget;

    public Transform player;
    public GameObject crosshair;
    public Transform aimPosition;

    [Header("Настройки")]
    public Vector3 aimOffset = new Vector3(0.5f, 1.65f, 00.6f);
    public bool isAiming = false;

    private void Start()
    {
        crosshair.SetActive(false);
    }

    private void Update()
    {
        HandleAimingInput();
        UpdateAimPosition();
    }

    private void HandleAimingInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            EnterAimingMode();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ExitAimingMode();
        }
    }

    private void EnterAimingMode()
    {
        isAiming = true;
        thirdPersonCamera.SetState(new AimingCameraState(thirdPersonCamera, aimPosition, rigController, rigBuilder, animationRig, aimTarget));
        SetCrosshairVisibility(true);
    }

    private void ExitAimingMode()
    {
        isAiming = false;
        thirdPersonCamera.SetState(new NormalCameraState(thirdPersonCamera));
        SetCrosshairVisibility(false);
    }

    private void SetCrosshairVisibility(bool isVisible)
    {
        if (crosshair != null)
        {
            crosshair.SetActive(isVisible);
        }
    }


    private void UpdateAimPosition()
    {
        if (thirdPersonCamera != null && aimPosition != null && player != null)
        {
            Vector3 cameraForward = thirdPersonCamera.transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = thirdPersonCamera.transform.right;
            
            Vector3 horizontalOffset = cameraForward * aimOffset.z + cameraRight * aimOffset.x;
            Vector3 targetPosition = player.position + horizontalOffset;
            targetPosition.y = player.position.y + aimOffset.y;

            aimPosition.position = targetPosition;
            aimPosition.LookAt(thirdPersonCamera.GetLastAimPoint());
        }
    }
}