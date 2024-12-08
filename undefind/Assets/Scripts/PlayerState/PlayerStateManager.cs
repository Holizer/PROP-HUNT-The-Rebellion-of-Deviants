using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private IPlayerState currentState;
    public int Health { get; set; } = 100;
    public bool IsDead() => Health <= 0;

    public PlayerRole Role = PlayerRole.Hider;
    public IPlayerState CurrentState => currentState;

    public void Start()
    {
        currentState = new AliveState();
        currentState.EnterState(this);
    }

    public void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(IPlayerState newState)
    {
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    public void PauseGame()
    {
        if (!(currentState is PauseState) && !IsDead())
        {
            SwitchState(new PauseState());
        }
    }

    public void ResumeGame()
    {
        if (currentState is PauseState && !IsDead())
        {
            SwitchState(new AliveState());
        }
    }

    public void SetPlayerControls(bool enabled)
    {
        if (Role == PlayerRole.Hider)
        {
            GetComponent<HiderMovement>().enabled = enabled;
        }
        else if (Role == PlayerRole.Hunter)
        {
            GetComponent<HunterMovement>().enabled = enabled;
        }
        GetComponentInChildren<ThirdPersonCamera>().enabled = enabled;
    }
}
