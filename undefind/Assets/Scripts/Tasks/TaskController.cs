using UnityEngine;

public class TaskController : MonoBehaviour, ITask
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

        if (Vector3.Distance(performer.transform.position, taskObject.transform.position) < 3f)
        {
            taskCompleted = true;
            Debug.Log($"{performer.name} выполнил задание: {taskName}");
        }
    }

    public bool IsCompleted()
    {
        return taskCompleted;
    }
}