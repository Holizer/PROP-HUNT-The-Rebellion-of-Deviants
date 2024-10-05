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
public abstract class BaseCameraState : ICameraState
{
    protected ThirdPersonCamera camera;

    public BaseCameraState(ThirdPersonCamera camera)
    {
        this.camera = camera;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public abstract void UpdateState();
    public abstract void LateUpdateState();
}
