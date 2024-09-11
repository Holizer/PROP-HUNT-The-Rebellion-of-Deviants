using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Player
{
    public GameObject hunterModel;
    void Start()
    {
        Role = PlayerRole.Hunter;
    }

    public void Shoot()
    {
        Debug.Log("Hunter shooting!");
    }
}
