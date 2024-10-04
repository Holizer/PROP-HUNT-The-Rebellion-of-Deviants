using UnityEngine;

public class HunterAiming : MonoBehaviour
{
    [Header("Компоненты")]
    public ThirdPersonCamera camera;
    public Transform player;
    public GameObject crosshair;
    public Transform aimPosition;

    [Header("Настройки")]
    public float returnSpeed = 0.3f;
    public float aimDistance = 4f;
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
        camera.SetState(new ReturningCameraState(camera, aimPosition, returnSpeed));
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
        if (camera != null && aimPosition != null && !isAiming)
        {
            // Вращаем aimPosition вокруг игрока, следуя за камерой
            aimPosition.position = player.TransformPoint(aimOffset);

            // Вращаем aimPosition вокруг игрока, следуя за камерой
            aimPosition.RotateAround(player.position, Vector3.up, camera.transform.eulerAngles.y);

            // Обновляем позицию и вращение aimPosition
            aimPosition.position -= camera.transform.forward * aimDistance;
            aimPosition.rotation = camera.transform.rotation;
        }
    }
}
