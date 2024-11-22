using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [Header("Лист заданий")]
    public TaskList taskList;
    
    private List<Task> tasks = new List<Task>();

    private void HandleTaskCompleted(Task completedTask)
    {
        Debug.Log($"Задание {completedTask.TaskName} завершено! Обновляем статус.");
        taskList.UpdateTaskUI(tasks);

    }
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
                    task.OnTaskCompleted += HandleTaskCompleted;
                    
                    Debug.Log($"Задание найдено: {task.TaskName}");
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
}
