using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<ITask> tasks = new List<ITask>();
    [SerializeField] private GameObject currentPerformer;

    public GameObject taskContainer;

    void Start()
    {
        InitializeTasks();
    }

    void InitializeTasks()
    {
        if (taskContainer != null)
        {
            foreach (Transform taskObject in taskContainer.transform)
            {
                Debug.Log("taskObject: " + taskObject.name); 
                TaskController task = taskObject.GetComponent<TaskController>();

                if (task != null)
                {
                    task.Initialize(taskObject.gameObject); 
                    tasks.Add(task); 
                }
                else
                {
                    Debug.LogWarning($"Задание на объекте {taskObject.name} не имеет компонента TaskController.");
                }
            }
        }
        else
        {
            Debug.LogError("TaskContainer не найден!"); 
        }
    }


    void Update()
    {
        if (currentPerformer != null)
        {
            foreach (var task in tasks)
            {
                if (!task.IsCompleted())
                {
                    task.PerformTask(currentPerformer);
                }
            }
        }
    }

    public void SetCurrentPerformer(GameObject performer)
    {
        currentPerformer = performer;
    }
}