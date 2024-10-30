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
        Debug.Log("Сын Пиллея");
        hunterMovement.enabled = true;
        hunterAnimation.enabled = true;
        camera.SetActive(true);
        hunterShoot.enabled = true;
        hunterAimPosition.enabled = true;
    }
    //public GameObject playerModel { get; protected set; }

    //public virtual void Initialize(GameObject player, GameObject model, PlayerRole Role)
    //{
    //    this.Role = Role;
        
    //    if (model == null)
    //    {
    //        Debug.LogError($"{Role} model is not assigned!");
    //        return;
    //    }
    //    playerModel = model;
    //    playerModel.SetActive(true);
    //    Debug.Log($"{Role} initialized!");
    //}
}
