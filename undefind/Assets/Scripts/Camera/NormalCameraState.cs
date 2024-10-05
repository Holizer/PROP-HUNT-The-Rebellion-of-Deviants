using TMPro;
using UnityEngine;

public class NormalCameraState : BaseCameraState
{
    private float smoothTime = 0.015f;
    private float rotationSpeed = 8f;
    private Vector3 velocity = Vector3.zero;
    private bool isReturningToNormal = false;
    protected Transform player;

    public NormalCameraState(ThirdPersonCamera camera) : base(camera) {
        this.player = camera.player;
    }

    public override void EnterState()
    {
        isReturningToNormal = true;
    }

    public override void ExitState()
    {
        isReturningToNormal = false;
    }

    public override void UpdateState()
    {
        if (!isReturningToNormal)
        {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");
            camera.UpdateRotation(deltaX, deltaY);
        }
    }

    public override void LateUpdateState()
    {
        Quaternion rotation = camera.GetRotation();
        Vector3 desiredPosition = player.position + rotation * new Vector3(0, 0, -camera.distance);
        Quaternion desiredRotation = Quaternion.LookRotation(player.position + Vector3.up * 1.5f - camera.transform.position);

        camera.HandleCollision(ref desiredPosition);

        if (isReturningToNormal)
        {
            Vector3 smoothedPosition = Vector3.SmoothDamp(camera.transform.position, desiredPosition, ref velocity, smoothTime);
            camera.transform.position = smoothedPosition;

            Quaternion smoothedQuaternion = Quaternion.Slerp(camera.transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
            camera.transform.rotation = smoothedQuaternion;
            float angleDifference = Quaternion.Angle(camera.transform.rotation, desiredRotation);

            if (Vector3.Distance(camera.transform.position, desiredPosition) < 0.01f && angleDifference < 0.01f)
            {
                camera.transform.rotation = desiredRotation;
                isReturningToNormal = false;
            }
        }
        else
        {
            camera.transform.position = desiredPosition;
            camera.transform.rotation = Quaternion.LookRotation(player.position + Vector3.up * 1.5f - camera.transform.position);
        }
    }
}