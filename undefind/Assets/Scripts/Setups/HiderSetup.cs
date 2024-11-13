using Photon.Pun;
using UnityEngine;

public class HiderSetup : MonoBehaviour
{
    public HiderMovement hiderMovement;
    public HiderAnimation hiderAnimation;
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
        hiderMovement.enabled = true;
        hiderAnimation.enabled = true;
        cameraRig.SetActive(true);
    }
}
