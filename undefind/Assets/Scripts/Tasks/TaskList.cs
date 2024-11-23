using System.Collections.Generic;
using UnityEngine;

public class TaskList : MonoBehaviour
{
    [Header("Параметры задач")]
    [SerializeField] private GameObject taskItemUIPrefab;
    [SerializeField] private Transform taskUIContainer;

    private List<GameObject> taskUIObjects = new List<GameObject>();
    public void UpdateTaskUI(List<Task> tasks)
    {
        foreach (var uiObject in taskUIObjects)
        {
            Destroy(uiObject);
        }
        taskUIObjects.Clear();

        foreach (var task in tasks)
        {
            GameObject taskUI = Instantiate(taskItemUIPrefab, taskUIContainer);
            TaskUI taskUIComponent = taskUI.GetComponent<TaskUI>();

            if (taskUIComponent != null)
            {
                string taskName = task.taskName;
                string taskStatus = task.IsCompleted() ? "1/1" : "0/1";
                taskUIComponent.Initialize(taskName, taskStatus);
            }

            taskUIObjects.Add(taskUI);
        }
    }
}