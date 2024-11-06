using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public HunterMovement hunterMovement;

    public HunterAnimation hunterAnimation;

    public HunterShoot hunterShoot;
    
    public HunterAimPosition hunterAimPosition;

    public GameObject camera;
    public PlayerRole Role { get; protected set; }

    public void IsLocalPlayer()
    {
        hunterMovement.enabled = true;
        hunterAnimation.enabled = true;
        camera.SetActive(true);
        hunterShoot.enabled = true;
        hunterAimPosition.enabled = true;
    }
}
