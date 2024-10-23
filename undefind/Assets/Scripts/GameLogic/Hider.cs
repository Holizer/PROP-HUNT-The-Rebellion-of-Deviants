using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Hider : Player
{
    public GameObject hiderModel;

    public void Initialize(GameObject player, GameObject model)
    {
        Role = PlayerRole.Hider;
        if (model != null)
        {
            hiderModel = model;
            hiderModel.SetActive(true);
            Debug.Log("Hider проинциализирован!");
        }
        else
        {
            Debug.LogError("hiderModel не назначен!");
        }
     
        player.AddComponent<HiderMovement>();
    }

    //private void ConfigureHiderMovement(GameObject model, CharacterController characterController)
    //{
    //    Animator animator = model.GetComponent<Animator>();
    //    if (animator != null)
    //    {
    //        animator.enabled = true;
    //    }
    //    HiderAnimations hiderAnimations = model.GetComponent<HiderAnimations>();
    //    if (hiderAnimations != null)
    //    {
    //        hiderAnimations.enabled = true;
    //    }

    //    Transform cameraRig = currentPlayer.transform.Find("CameraRig");
    //    HiderMovement hiderMovement = model.GetComponent<HiderMovement>();
    //    if (hiderMovement != null)
    //    {
    //        hiderMovement.cameraTransform = cameraRig;
    //        hiderMovement.controller = characterController;
    //        hiderMovement.enabled = true;
    //    }

    //    ThirdPersonCamera thirdPersonCamera = cameraRig.GetComponent<ThirdPersonCamera>();
    //    thirdPersonCamera.player = currentPlayer.transform;
    //    thirdPersonCamera.enabled = true;

    //    GameObject aimPosition = currentPlayer.transform.Find("AimPosition").gameObject;
    //    Destroy(aimPosition);
    //}
}
