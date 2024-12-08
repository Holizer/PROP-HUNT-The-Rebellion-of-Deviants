using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class AliveState : IPlayerState
{
    public void EnterState(PlayerStateManager player)
    {
        Debug.Log("Игрок жив! Можно управлять персонажем.");
        player.SetPlayerControls(true);
    }

    public void UpdateState(PlayerStateManager player)
    {
        if (player.Health <= 0)
        {
            player.SwitchState(new DeadState());
        }
    }

    public void ExitState(PlayerStateManager player)
    {
        Debug.Log("Игрок больше не жив.");
    }

    public bool IsApplicable(PlayerStateManager player) => true;
}
