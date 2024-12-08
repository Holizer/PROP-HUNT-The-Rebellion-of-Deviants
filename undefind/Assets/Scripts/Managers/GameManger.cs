using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [SerializeField] private TaskManager taskManager;
    private bool hiderAlive = true;
    private bool gameEnded = false;

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
        Debug.Log("gameEnded:" + gameEnded);
        if (gameEnded) return;

        hiderAlive = isAlive;
        Debug.Log("hiderAlive:" + hiderAlive);

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
        Debug.Log($"Победила команда: {winningTeam}!");
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
