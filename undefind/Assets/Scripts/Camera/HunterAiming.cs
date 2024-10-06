using UnityEngine;

public class HunterAiming : MonoBehaviour
{
    [Header("Компоненты")]
    public ThirdPersonCamera camera;
    public Transform player;
    public GameObject crosshair;
    public Transform aimPosition;

    [Header("Настройки")]
    public float aimDistance = 1f;
    public Vector3 aimOffset = new Vector3(0, 1, 0);

    public bool isAiming = false;

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
        camera.SetState(new AimingCameraState(camera, aimPosition));
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
        if (camera != null && aimPosition != null)
        {
            aimPosition.position = player.TransformPoint(aimOffset);
            aimPosition.RotateAround(player.position, Vector3.up, camera.transform.eulerAngles.y);

            aimPosition.position -= camera.transform.forward * aimDistance;
            aimPosition.rotation = camera.transform.rotation;
        }
    }
}
