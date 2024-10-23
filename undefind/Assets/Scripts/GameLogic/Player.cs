using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerRole Role { get; protected set; }

    public GameObject playerModel { get; protected set; }

    public virtual void Initialize(GameObject player, GameObject model, PlayerRole Role)
    {
        this.Role = Role;
        
        if (model == null)
        {
            Debug.LogError($"{Role} model is not assigned!");
            return;
        }
        playerModel = model;
        playerModel.SetActive(true);
        Debug.Log($"{Role} initialized!");
    }
}
