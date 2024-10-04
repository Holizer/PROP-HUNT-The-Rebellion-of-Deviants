using TMPro;
using UnityEngine;

public class NormalCameraState : ICameraState
{
    private ThirdPersonCamera camera;
    private Transform player;
    public NormalCameraState(ThirdPersonCamera camera)
    {
        this.camera = camera;
        this.player = camera.player;
    }
    public void EnterState() { }
    public void ExitState() { }

    public void UpdateState()
    {
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");
        camera.UpdateRotation(deltaX, deltaY);
    }

    public void LateUpdateState()
    {
        Quaternion rotation = camera.GetRotation();
        Vector3 desiredPosition = player.position + rotation * new Vector3(0, 0, -camera.distance);
        camera.HandleCollision(desiredPosition);
        camera.LookAt(player.position + Vector3.up * 1.5f);
    }
}
