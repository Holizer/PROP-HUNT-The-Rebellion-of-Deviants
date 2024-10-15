using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimingCameraState : BaseCameraState
{
    private Transform aimPosition;
    private GameObject aimTarget;
    private Vector3 currentVelocity = Vector3.zero;
    private float smoothTime = 0.01f;

    private float aimingSensitivity = 60f;
    private bool aimingInvertY = false;
    private float aimingYMinLimit = -20f;
    private float aimingYMaxLimit = 20f;

    //private static int aimTargetCounter = 0;

    public AimingCameraState(ThirdPersonCamera camera, Transform aimPosition) : base(camera)
    {
        this.aimPosition = aimPosition;
        this.aimingInvertY = camera.invertY;
    }

    public override void EnterState()
    {
        camera.animationRig.weight = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 targetAimingPoint = camera.AimingRay();
        //aimTarget = new GameObject($"AimTarget_{aimTargetCounter++}");
        //aimTarget.transform.position = targetAimingPoint;

        //WeightedTransform weightedTransform = new WeightedTransform
        //{
        //    transform = aimTarget.transform,
        //    weight = 1.0f
        //};

        //foreach (var constraint in camera.aimConstraints)
        //{
        //    constraint.data.sourceObjects = new WeightedTransformArray();
        //    constraint.data.sourceObjects.Add(weightedTransform);
        //}

        SetNewCameraAngles(targetAimingPoint);
    }

    public override void ExitState()
    {
        camera.animationRig.weight = 0;
        //camera.animationRig.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Destroy the AimTarget when exiting the aiming state
        if (aimTarget != null)
        {
            foreach (var constraint in camera.aimConstraints)
            {
                WeightedTransform weightedTransform = new WeightedTransform
                {
                    transform = aimTarget.transform,
                    weight = 1.0f
                };
                constraint.data.sourceObjects.Remove(weightedTransform);
            }
            GameObject.Destroy(aimTarget);
        }
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
        camera.aimTarget.position = targetAimingPoint;
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