using UnityEngine;

public abstract class Task : MonoBehaviour, ITask
{
    public abstract string TaskName { get; }

    protected GameObject targetObject;
    
    protected bool taskCompleted;
    
    protected TaskType taskType;
    
    public event System.Action<Task> OnTaskCompleted;

    public virtual void Initialize(GameObject targetObject = null)
    {
        this.targetObject = targetObject;
        taskCompleted = false;

        if (this.targetObject == null)
        {
            Debug.LogWarning("targetObject �� ��� ���������� ��� �������.");
        }
    }


    public virtual void PerformTask(GameObject performer)
    {
        if (taskCompleted) return;
        
        Debug.Log($"������� {taskType} ����������� ��� {performer.name}");
    }
    protected void CompleteTask()
    {
        taskCompleted = true;
        Debug.Log($"������� ���������!");
        OnTaskCompleted?.Invoke(this);
    }

    public bool IsCompleted() => taskCompleted;

    public TaskType GetTaskType() => taskType;

}
