using Photon.Pun;
using UnityEngine;

public class HunterSetup : MonoBehaviour
{
    public HunterMovement hunterMovement;
    public HunterAnimation hunterAnimation;
    public HunterShoot hunterShoot;
    public HunterAimPosition hunterAimPosition;
    public GameObject cameraRig;
    public PlayerRole Role { get; protected set; }

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            SetupLocalPlayer();
        }
    }

    public void SetupLocalPlayer()
    {
        hunterMovement.enabled = true;
        hunterAnimation.enabled = true;
        cameraRig.SetActive(true);
        hunterShoot.enabled = true;
        hunterAimPosition.enabled = true;
    }
}
