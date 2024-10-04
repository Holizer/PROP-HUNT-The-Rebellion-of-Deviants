using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturningCameraState : ICameraState
{
    private ThirdPersonCamera camera;
    private Transform player;
    private Transform aimPosition;
    private float lerpTime;
    private float elapsedTime;

    public ReturningCameraState(ThirdPersonCamera camera, Transform aimPosition, float lerpTime = 0.3f)
    {
        this.camera = camera;
        this.player = camera.player;
        this.aimPosition = aimPosition;
        this.lerpTime = lerpTime;
        this.elapsedTime = 0f;
    }
    public void EnterState()
    {
        elapsedTime = 0f;
    }

    public void ExitState()
    {
    }

    public void UpdateState()
    {
    }

    public void LateUpdateState()
    {
        Vector3 targetPosition = aimPosition.position - aimPosition.forward * camera.distance;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / lerpTime);

        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, t);

        Quaternion targetRotation = Quaternion.LookRotation(aimPosition.position - camera.transform.position);
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, t);

        if (t >= 1f)
        {
            camera.SetState(new NormalCameraState(camera));
        }
    }
}
