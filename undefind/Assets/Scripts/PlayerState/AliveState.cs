using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class AliveState : IPlayerState
{
    public void EnterState(PlayerStateManager player)
    {
        player.SetPlayerControls(true);
    }

    public void UpdateState(PlayerStateManager player)
    {
        if (player.Health <= 0)
        {
            player.SwitchState(new DeadState());
        }
    }

    public void ExitState(PlayerStateManager player) { }

    public bool IsApplicable(PlayerStateManager player) => true;
}
