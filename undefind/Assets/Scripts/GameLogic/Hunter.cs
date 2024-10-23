using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Hunter : Player
{
    public override void Initialize(GameObject player, GameObject model, PlayerRole role = PlayerRole.Hunter)
    {
        base.Initialize(player, model, PlayerRole.Hunter);
        AddAndConfigureComponents(player);
    }

    private void AddAndConfigureComponents(GameObject player)
    {
        AddMovementComponent(player);
        ConfigureComponents(player, playerModel);
    }

    private void AddMovementComponent(GameObject player)
    {
        if (!player.TryGetComponent(out CharacterController characterController))
        {
            characterController = player.AddComponent<CharacterController>();
        }

        if (!player.TryGetComponent(out HunterMovement hunterMovement))
        {
            hunterMovement = player.AddComponent<HunterMovement>();
            hunterMovement.controller = characterController;
            Transform cameraRig = player.transform.Find("CameraRig");
            if (cameraRig == null)
            {
                Debug.LogError("CameraRig not found!");
                return;
            }
            hunterMovement.cameraTransform = cameraRig;
        }
    }

    private void ConfigureComponents(GameObject player, GameObject model)
    {
        ConfigureAnimator(playerModel);
        ConfigureCamera(player, playerModel);
        ConfigureAiming(player, playerModel);
    }

    private void ConfigureAnimator(GameObject model)
    {
        if (model.TryGetComponent(out Animator animator))
        {
            animator.enabled = true;
        }

        if (model.TryGetComponent(out HunterAnimation hunterAnimation))
        {
            hunterAnimation.enabled = true;
        }
    }

    private void ConfigureCamera(GameObject player, GameObject model)
    {
        Transform cameraRig = player.transform.Find("CameraRig");
        if (cameraRig == null)
        {
            Debug.LogError("CameraRig not found!");
            return;
        }

        ThirdPersonCamera camera = cameraRig.GetComponent<ThirdPersonCamera>();
        if (camera == null)
        {
            Debug.LogError("ThirdPersonCamera not found!");
            return;
        }

        camera.player = player.transform;
        camera.enabled = true;

        Transform rigTransform = model.transform.Find("Rig");
        if (rigTransform != null && rigTransform.TryGetComponent(out Rig rigComponent))
        {
            camera.animationRig = rigComponent;
        }
    }

    private void ConfigureAiming(GameObject player, GameObject model)
    {
        Transform aimPosition = player.transform.Find("AimPosition");
        if (aimPosition == null)
        {
            Debug.LogError("AimPosition not found!");
            return;
        }

        if (aimPosition.TryGetComponent(out HunterAiming hunterAiming))
        {
            hunterAiming.player = player.transform;
            hunterAiming.camera = model.GetComponent<ThirdPersonCamera>();
            hunterAiming.aimPosition = aimPosition;
            hunterAiming.enabled = true;
        }
        else
        {
            Debug.LogError("HunterAiming not found on AimPosition!");
        }
    }
}