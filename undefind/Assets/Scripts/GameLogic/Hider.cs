using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Hider : Player
{
    public GameObject hiderModel;

    public void Initialize(GameObject model)
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
    }
}
