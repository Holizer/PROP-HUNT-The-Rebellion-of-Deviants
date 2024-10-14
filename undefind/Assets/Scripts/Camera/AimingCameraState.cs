using UnityEngine;
using UnityEngine.VFX;

public class AimingCameraState : BaseCameraState
{
    private Transform aimPosition;
    private Vector3 currentVelocity = Vector3.zero;
    private float smoothTime = 0.01f;

    private float aimingSensitivity = 60f;
    private bool aimingInvertY = false;
    private float aimingYMinLimit = -20f;
    private float aimingYMaxLimit = 20f;

    public AimingCameraState(ThirdPersonCamera camera, Transform aimPosition) : base(camera)
    {
        this.aimPosition = aimPosition;
        this.aimingInvertY = camera.invertY;
    }

    public override void EnterState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 targetAimingPoint = camera.AimingRay();
        SetNewCameraAngles(targetAimingPoint);
    }

    public override void ExitState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void UpdateState()
    {
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        camera.UpdateRotation(deltaX, deltaY, aimingSensitivity, aimingInvertY, aimingYMinLimit, aimingYMaxLimit);
    }

    public override void LateUpdateState()
    {
        Vector3 targetPosition = aimPosition.position;
        camera.SmoothPosition(Vector3.SmoothDamp(camera.transform.position, targetPosition, ref currentVelocity, smoothTime));

        camera.SetRotation();
        Vector3 targetAimingPoint = camera.AimingRay();
        camera.LookAt(targetAimingPoint);
    }

    private void SetNewCameraAngles(Vector3 targetAimingPoint)
    {
        Vector3 direction = targetAimingPoint - camera.transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        Vector3 euler = lookRotation.eulerAngles;
        camera.currentX = euler.y;
        camera.currentY = euler.x;
    }
}