using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private IPlayerState currentState;
    public int Health { get; set; } = 100;
    public bool IsDead() => Health <= 0;

    public PlayerRole Role = PlayerRole.Hider;
    public IPlayerState CurrentState => currentState;

    [SerializeField] private UIManager uiManager;

    public void Start()
    {
        currentState = new AliveState();
        currentState.EnterState(this);
        
        if (uiManager == null)
        {
            uiManager = GetComponentInChildren<UIManager>();
        }
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

    public void ShowEndGameMessage(string message, Color color)
    {
        uiManager.DisplayMessage(message, color);
    }
}
