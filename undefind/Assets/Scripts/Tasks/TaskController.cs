using UnityEngine;

public class TaskController : MonoBehaviour
{
    public string taskName;
    public bool taskCompleted;
    public GameObject taskObject;

    public void Initialize(GameObject taskObj)
    {
        taskName = taskObj.name;
        taskObject = taskObj;
    }

    public void PerformTask(GameObject performer)
    {
        if (taskCompleted) return;
    }

    public bool IsCompleted()
    {
        return taskCompleted;
    }
}