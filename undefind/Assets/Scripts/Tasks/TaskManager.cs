using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    [Header("���� �������")]
    public TaskList taskList;
    
    private List<Task> tasks = new List<Task>();

    private void HandleTaskCompleted(Task completedTask)
    {
        Debug.Log($"������� {completedTask.TaskName} ���������! ��������� ������.");
        taskList.UpdateTaskUI(tasks);

    }
    private void Start()
    {
        if (taskList == null)
        {
            Debug.LogError("TaskList �� ������ � �����!");
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
                    
                    Debug.Log($"������� �������: {task.TaskName}");
                }
            }

            if (taskList != null)
            {
                taskList.UpdateTaskUI(tasks);
            }
            else
            {
                Debug.LogError("TaskList �� �������� � TaskManager!");
            }
        }
        else
        {
            Debug.LogError("TaskContainer �� ������ �� �����! ���������, ��� ������ ���������� � ������ ���������.");
        }
    }
}
