using UnityEngine;

public class AimingCameraState : BaseCameraState
{
    private Transform aimPosition;
    private Vector3 currentVelocity = Vector3.zero;
    private float smoothTime = 0.01f;

    public AimingCameraState(ThirdPersonCamera camera, Transform aimPosition) : base(camera) {
        this.aimPosition = aimPosition;
    }

    public override void EnterState()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void ExitState()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void UpdateState() { }

    public override void LateUpdateState()
    {

        Vector3 targetPosition = aimPosition.position;
        camera.SmoothPosition(Vector3.SmoothDamp(camera.transform.position, targetPosition, ref currentVelocity, smoothTime));
        
        Vector3 targetAimingPoint = camera.AimingRay();
        camera.LookAt(targetAimingPoint);
    }
    
    private void UpdateHands()
    {
        // Assuming you have references to the hand transforms
        Transform rightHand = camera.player.Find("RightHand");
        Transform leftHand = camera.player.Find("LeftHand");

        if (rightHand == null || leftHand == null)
        {
            Debug.LogError("Не удалось найти трансформы рук.");
            return;
        }

        // Calculate the direction to point the weapon
        Vector3 aimDirection = camera.AimingRay() - rightHand.position;
        Debug.Log("Направление прицеливания: " + aimDirection);

        // Smoothly rotate the hands to point the weapon
        Quaternion rightHandRotation = Quaternion.LookRotation(aimDirection);
        rightHand.rotation = Quaternion.Slerp(rightHand.rotation, rightHandRotation, smoothTime);
        Debug.Log("Поворот правой руки: " + rightHand.rotation);

        // Optionally, adjust the left hand similarly if needed
        // Quaternion leftHandRotation = Quaternion.LookRotation(aimDirection);
        // leftHand.rotation = Quaternion.Slerp(leftHand.rotation, leftHandRotation, smoothTime);
        // Debug.Log("Поворот левой руки: " + leftHand.rotation);
    }
}