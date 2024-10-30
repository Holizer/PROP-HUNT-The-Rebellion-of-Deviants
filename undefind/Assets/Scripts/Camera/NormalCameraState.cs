using TMPro;
using UnityEngine;

public class NormalCameraState : BaseCameraState
{
    protected Transform player;
    private bool isReturningToNormal = false;

    private float transitionDuration = 15f;
    private float transitionProgress = 0f;

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
        Vector3 heightOffset = new Vector3(0, 1.5f, 0);
        Vector3 standardOffset = new Vector3(0, 0, -camera.distance);

        Vector3 targetPosition = player.position + heightOffset + rotation * standardOffset;
        Quaternion targetRotation = Quaternion.LookRotation(player.position + heightOffset - camera.transform.position);

        if (isReturningToNormal)
        {
            if (player.GetComponent<HunterMovement>() != null)
            {
                player.GetComponent<HunterMovement>().isCameraTransition = true;
            }
            PerformTransitionToNormal(targetPosition, targetRotation);
        }
        else
        {
            // Позиция камеры будет рассчитана относительно игрока
            camera.transform.position = player.position + heightOffset + rotation * standardOffset;
            camera.transform.rotation = Quaternion.LookRotation(player.position + heightOffset - camera.transform.position);
        }
    }
    private void PerformTransitionToNormal(Vector3 desiredPosition, Quaternion desiredRotation)
    {
        transitionProgress += Time.deltaTime / transitionDuration;
        transitionProgress = Mathf.Clamp01(transitionProgress);
        float linearValue = transitionProgress;

        camera.transform.position = Vector3.Lerp(camera.transform.position, desiredPosition, linearValue);
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, desiredRotation, linearValue);

        if (Vector3.Distance(camera.transform.position, desiredPosition) < 0.01f &&
            Quaternion.Angle(camera.transform.rotation, desiredRotation) < 1f)
        {
            camera.transform.position = desiredPosition;
            camera.transform.rotation = desiredRotation;

            transitionProgress = 0f;
            isReturningToNormal = false;

            if (player.GetComponent<HunterMovement>() != null)
            {
                player.GetComponent<HunterMovement>().isCameraTransition = false;
            }
        }
    }
}