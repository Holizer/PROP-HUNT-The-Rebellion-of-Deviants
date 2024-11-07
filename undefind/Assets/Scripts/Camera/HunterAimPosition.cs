using Photon.Realtime;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HunterAimPosition : MonoBehaviour
{
    [Header("Компоненты")]
    public ThirdPersonCamera camera;

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
        camera.SetState(new AimingCameraState(camera, aimPosition, rigController, rigBuilder, animationRig, aimTarget));
        SetCrosshairVisibility(true);
    }

    private void ExitAimingMode()
    {
        isAiming = false;
        camera.SetState(new NormalCameraState(camera));
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
        if (camera != null && aimPosition != null && player != null)
        {
            Vector3 cameraForward = camera.transform.forward;
            cameraForward.y = 0f;
            Vector3 cameraRight = camera.transform.right;
            
            Vector3 horizontalOffset = cameraForward * aimOffset.z + cameraRight * aimOffset.x;
            Vector3 targetPosition = player.position + horizontalOffset;
            targetPosition.y = player.position.y + aimOffset.y;

            aimPosition.position = targetPosition;
            aimPosition.LookAt(camera.GetLastAimPoint());
        }
    }
}