using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Header("Лист заданий")]
    public TaskList taskList;
    private List<Task> tasks = new List<Task>();

    [Header("Персонаж игрока или бота")]
    public GameObject performer;

    [Header("UI Подсказака управления")]
    public GameObject taskHintUI;
    private Task currentTask;

    private void Start()
    {
        if (taskList == null)
        {
            Debug.LogError("TaskList не найден в сцене!");
        }
        InitializeTasks();
    }

    private void InitializeTasks()
    {
        GameObject taskContainerObject = GameObject.Find("TaskContainer");

        if (taskContainerObject != null)
        {
            Transform taskContainer = taskContainerObject.transform;

            tasks.Clear();
            foreach (Transform child in taskContainer)
            {
                Task task = child.GetComponent<Task>();
                if (task != null)
                {
                    tasks.Add(task);
                    task.Initialize(gameObject);
                    SetTaskZoneEventHandlers(task);
                }
            }

            if (taskList != null)
            {
                taskList.UpdateTaskUI(tasks);
            }
            else
            {
                Debug.LogError("TaskList не привязан к TaskManager!");
            }
        }
        else
        {
            Debug.LogError("TaskContainer не найден на сцене! Убедитесь, что объект существует и назван корректно.");
        }
    }
    #region TaskZone

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private void SetTaskZoneEventHandlers(Task task)
    {
        TaskZone taskZone = task.GetComponentInChildren<TaskZone>();
        if (taskZone != null)
        {
            taskZone.OnPlayerEnteredZone += HandlePlayerEnteredZone;
            taskZone.OnPlayerLeftZone += HandlePlayerLeftZone;
        }
    }

    private void HandleTaskCompleted(Task completedTask)
    {
        taskList.UpdateTaskUI(tasks);
    }
    private void HandlePlayerLeftZone(Task task)
    {
        currentTask = null;
        if (taskHintUI != null)
        {
            taskHintUI.SetActive(false);
        }
    }
    private void HandlePlayerEnteredZone(Task task)
    {
        currentTask = task;
        if (taskHintUI != null)
        {
            taskHintUI.SetActive(true);
        }

        TurnPlayerToTask(task);
    }

    private void TurnPlayerToTask(Task task)
    {
        if (performer != null)
        {
            Vector3 targetPosition = task.transform.position; 
            Vector3 direction = targetPosition - performer.transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction); 
            performer.transform.rotation = targetRotation; 
        }
    }

    #endregion
    private void Update()
    {
        if (currentTask != null && taskHintUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            PerformTask(currentTask);
        }
    }

    private void PerformTask(Task task)
    {
        GameObject performer = GameObject.FindGameObjectWithTag("Hider");

        if (performer != null)
        {
            task.PerformTask(performer);
            taskHintUI.SetActive(false); 
        }
    }
}