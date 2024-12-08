using Unity.Burst.CompilerServices;
using UnityEngine;

public class DeadState : IPlayerState
{
    public void EnterState(PlayerStateManager player)
    {
        player.GetComponent<HiderMovement>().enabled = false;
        player.GetComponent<HiderAnimation>().enabled = false;
        Animator animator = player.GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsDeath", true);
        }
        Debug.Log("����� �����! �������� ����� ������.");
    }

    public void UpdateState(PlayerStateManager player) { }

    public void ExitState(PlayerStateManager player)
    {
        Debug.Log("����� ������������ ��� ������� �� �������� ���������.");
    }

    public bool IsApplicable(PlayerStateManager player)
    {
        return player.Role == PlayerRole.Hider; 
    }
}
