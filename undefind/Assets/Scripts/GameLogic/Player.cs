using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsLocalPlayer { get; protected set; }

    public PlayerRole Role { get; protected set; }

    void Awake()
    {
    }

    public void SetLocalPlayer(bool isLocal)
    {
        IsLocalPlayer = isLocal;
    }
    public void SetRole(PlayerRole playerRole)
    {
        Role = playerRole;
    }
}
