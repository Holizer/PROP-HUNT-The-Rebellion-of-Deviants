using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraState
{
    void UpdateState();
    void LateUpdateState();
    void EnterState();
    void ExitState();
}

public abstract class CameraState : ICameraState
{
    protected ThirdPersonCamera camera;
    protected Transform player;

    public CameraState(ThirdPersonCamera camera)
    {
        this.camera = camera;
        this.player = camera.player;
    }

    public virtual void EnterState() { }

    public virtual void ExitState() { }

    public virtual void UpdateState() { }

    public virtual void LateUpdateState() { }
}