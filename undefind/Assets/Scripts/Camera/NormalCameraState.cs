using TMPro;
using UnityEngine;

public class NormalCameraState : BaseCameraState
{
    protected Transform player;
    private bool isReturningToNormal = false;

    private float rotationSpeed = 0.09f;
    private float transformSpeed = 0.07f;
    private float transitionDuration = 0.9f;
    private float transitionProgress = 0f;
    private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public NormalCameraState(ThirdPersonCamera camera) : base(camera)
    {
        this.player = camera.player;
    }

    public override void EnterState()
    {
        isReturningToNormal = true;
        transitionProgress = 0f;
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
        
        if (isReturningToNormal)
        {
            transitionProgress += Time.deltaTime / transitionDuration;
            float curveValue = transitionCurve.Evaluate(transitionProgress);

            camera.transform.position = Vector3.Lerp(camera.transform.position, desiredPosition, curveValue * transformSpeed);
            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, desiredRotation, curveValue * rotationSpeed);
         
            if (transitionProgress >= 1f)
            {
                camera.transform.position = desiredPosition;
                camera.transform.rotation = desiredRotation;
                isReturningToNormal = false;
            }
        }
        else
        {
            desiredPosition = camera.HandleCollision(desiredPosition);
            camera.transform.position = desiredPosition;
            camera.transform.rotation = Quaternion.LookRotation(player.position + Vector3.up * 1.5f - camera.transform.position);
        }
    }
}