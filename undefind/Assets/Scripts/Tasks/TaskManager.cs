using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<ITask> tasks = new List<ITask>();

    void Start()
    {
        tasks.AddRange(GetComponentsInChildren<ITask>());
    }

    public void UpdateTasks(GameObject performer)
    {
        foreach (var task in tasks)
        {
            if (!task.IsCompleted())
            {
                task.PerformTask(performer);
            }
        }
    }
}
