using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void EnterState(PlayerStateManager player);
    void UpdateState(PlayerStateManager player);
    void ExitState(PlayerStateManager player);
    bool IsApplicable(PlayerStateManager player);
}