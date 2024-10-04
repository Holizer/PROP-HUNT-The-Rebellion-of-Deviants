using UnityEngine;

public class AimingCameraState : ICameraState
{
    private ThirdPersonCamera camera;
    private Transform aimPosition;
    private Vector3 currentVelocity = Vector3.zero;
    private float smoothTime = 0.02f;

    public AimingCameraState(ThirdPersonCamera camera, Transform aimPosition)
    {
        this.camera = camera;
        this.aimPosition = aimPosition;
    }

    public void EnterState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ExitState()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UpdateState() { }

    public void LateUpdateState()
    {
        Vector3 targetPosition = aimPosition.position;
        camera.SmoothPosition(Vector3.SmoothDamp(camera.transform.position, targetPosition, ref currentVelocity, smoothTime));
        camera.LookAt(aimPosition.position);
    }
}