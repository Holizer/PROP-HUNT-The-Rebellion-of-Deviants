using UnityEngine;

public class PauseState : IPlayerState
{
    public void EnterState(PlayerStateManager player)
    {
        Debug.Log("���� �� �����. ���� � ���� �����.");
        player.SetPlayerControls(false);
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
        Debug.Log("����� �� �����.");
        if (player.Health <= 0)
        {
            player.SwitchState(new AliveState());
        }
    }

    public bool IsApplicable(PlayerStateManager player) => true;
}
