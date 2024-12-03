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
        Debug.Log("TaskManager был назначен вручную.");
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
        Debug.Log($"Победила команда: {winningTeam}!");
        photonView.RPC("NotifyGameEnd", RpcTarget.All, winningTeam);
    }

    [PunRPC]
    private void NotifyGameEnd(string winningTeam)
    {
        gameEnded = true;
        Debug.Log($"Игра завершена! Победила команда: {winningTeam}");
    }
}
