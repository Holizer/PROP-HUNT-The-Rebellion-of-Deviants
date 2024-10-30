using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimingCameraState : BaseCameraState
{
    private Transform aimPosition;

    // Обработка анимации
    private Rig animationRig;
    private RigController rigController;
    private RigBuilder rigBuilder;
    private GameObject aimTarget;

    // Скорость перехода в камеры в aimPosition
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