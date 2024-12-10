using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField] private TaskManager taskManager;
    private bool hiderAlive = true;
    public bool gameEnded { get; private set; } = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AssignTaskManager(TaskManager newTaskManager)
    {
        taskManager = newTaskManager;
    }

    public void SetHiderStatus(bool isAlive)
    {
        if (gameEnded) return;

        hiderAlive = isAlive;

        if (!hiderAlive)
        {
            EndGame("Hunter");
        }
    }

    public void CheckTasksCompletion()
    {
        if (gameEnded) return;

        if (taskManager != null && taskManager.AreAllTasksCompleted() && hiderAlive)
        {
            EndGame("Hider");
        }
    }

    public void EndGame(string winningTeam)
    {
        gameEnded = true;
        photonView.RPC("NotifyGameEnd", RpcTarget.All, winningTeam);
    }

    [PunRPC]
    private void NotifyGameEnd(string winningTeam)
    {
        gameEnded = true;
        string message;
        Color messageColor;

        foreach (var player in FindObjectsOfType<PlayerStateManager>())
        {
            if (player.Role == PlayerRole.Hider)
            {
                if (winningTeam == "Hider")
                {
                    message = "Вы победили!";
                    messageColor = Color.green; 
                }
                else
                {
                    message = "Вы проиграли!";
                    messageColor = Color.red;
                }
            }
            else 
            {
                if (winningTeam == "Hunter")
                {
                    message = "Вы победили!";
                    messageColor = Color.green; 
                }
                else
                {
                    message = "Вы проиграли!";
                    messageColor = Color.red;  
                }
            }

            player.ShowEndGameMessage(message, messageColor);
        }
    }
}
