using UnityEngine;

public class Hunter : Player
{
    public GameObject hunterModel;

    public void Initialize(GameObject model)
    {
        Role = PlayerRole.Hunter;
        if (model != null)
        {
            hunterModel = model;
            hunterModel.SetActive(true);
            Debug.Log("Hunter �����������������!");
        }
        else
        {
            Debug.LogError("hunterModel �� ��������!");
        }
    }

    public void Shoot()
    {
        Debug.Log("Hunter shooting!");
    }
}
