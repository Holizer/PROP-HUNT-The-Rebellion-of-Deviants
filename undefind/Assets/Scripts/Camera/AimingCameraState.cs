using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimingCameraState : BaseCameraState
{
    private Transform aimPosition;

    // ��������� ��������
    private Rig animationRig;
    private RigController rigController;
    private RigBuilder rigBuilder;
    private GameObject aimTarget;

    // �������� �������� � ������ � aimPosition
    private float smoothTime = 0.3f;
    
    private float aimingSensitivity = 60f;
    private bool aimingInvertY = false;
    private float aimingYMinLimit = -20f;
    private float aimingYMaxLimit = 20f;

    public AimingCameraState(ThirdPersonCamera camera, Transform aimPosition, RigController rigController, RigBuilder rigBuilder, Rig animationRig, GameObject aimTarget) : base(camera)
    {
        this.aimPosition = aimPosition;
        this.rigController = rigController;
        this.rigBuilder = rigBuilder;
        this.animationRig = animationRig;
        this.aimTarget = aimTarget;

        this.aimingInvertY = camera.invertY;
        this.animationRig = animationRig;
    }
    public override void EnterState()
    {
        animationRig.weight = 1;
        rigBuilder.Build();
        SetCursorState();
    }

    public override void ExitState()
    {
        animationRig.weight = 0;
        SetCursorState();
    }

    public override void UpdateState()
    {
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        camera.UpdateRotation(deltaX, deltaY, aimingSensitivity, aimingInvertY, aimingYMinLimit, aimingYMaxLimit);
    }

    public override void LateUpdateState()
    {
        SmoothMoveToAimPosition();

        Vector3 targetAimingPoint = camera.AimingRay();
        aimTarget.transform.position = targetAimingPoint;
        rigController.SetAimTargets(aimTarget.transform);
            
        camera.LookAt(targetAimingPoint);
        camera.SetRotation();

        //if (camera.IsAiming) // ��������������, ��� � ��� ���� �������� IsAiming � ������
        //{
        //    Vector3 cameraForward = cameraTransform.forward;
        //    cameraForward.y = 0f; // ���������� ������������ ������������
        //    cameraForward.Normalize();

        //    if (cameraForward != Vector3.zero)
        //    {
        //        float targetAngle = Mathf.Atan2(cameraForward.x, cameraForward.z) * Mathf.Rad2Deg;
        //        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f); // ���������� �������
        //    }
        //}
    }

    private void SmoothMoveToAimPosition()
    {
        Vector3 targetPosition = aimPosition.position;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, Time.deltaTime / smoothTime);
    }

    private void SetCursorState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}