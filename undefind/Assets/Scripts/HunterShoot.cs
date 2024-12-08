using Photon.Pun;
using UnityEngine;

public class HunterShoot : MonoBehaviourPunCallbacks
{
    public float range = 30f;
    public ThirdPersonCamera thirdPersonCamera;
    public ParticleSystem Muzzelflash;
    public GameObject Flareeffect;
    public float Muzzelforce = 30f;
    public AudioSource shootAudioSource;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && thirdPersonCamera.currentState is AimingCameraState)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        LocalShoot();

        photonView.RPC("NetworkShoot", RpcTarget.Others, thirdPersonCamera.transform.position, thirdPersonCamera.transform.forward);
    }

    private void LocalShoot()
    {
        Muzzelflash.Play();
        //shootAudioSource.Play();

        RaycastHit hit;
        if (Physics.Raycast(thirdPersonCamera.transform.position, thirdPersonCamera.transform.forward, out hit, range))
        {
            HandleHit(hit);
        }
    }

    private void HandleHit(RaycastHit hit)
    {
        Debug.Log($"��������� � {hit.collider}");
        if (hit.collider.CompareTag("Hider")) {
            GameManager.Instance.SetHiderStatus(false);
            PlayerStateManager playerStateManager = hit.collider.GetComponent<PlayerStateManager>();
            if (playerStateManager != null)
            {
                playerStateManager.Health -= 100;
            }
        }

        if (hit.collider.CompareTag("NPC"))
        {
            Animator animator = hit.collider.GetComponentInChildren<Animator>();
            if (animator != null)
            {
                animator.SetBool("IsDeath", true);
            }
        }

        if (hit.collider.CompareTag("Hunter"))
        {
            return; 
        }

        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(hit.normal * Muzzelforce);
        }

        GameObject hitEffect = Instantiate(Flareeffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(hitEffect, 2f);
    }

    [PunRPC]
    private void NetworkShoot(Vector3 position, Vector3 direction)
    {
        Muzzelflash.Play();
        //shootAudioSource.Play();

        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, range))
        {
            HandleHit(hit);
        }
    }
}
