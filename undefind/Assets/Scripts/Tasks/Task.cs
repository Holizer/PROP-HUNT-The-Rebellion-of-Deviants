using UnityEngine;

public abstract class Task : MonoBehaviour, ITask
{
    public abstract string taskName { get; }
    public GameObject taskObject { get; private set; }
    
    protected bool taskCompleted;
    
    protected TaskType taskType;

    public virtual string animationParameter => string.Empty;
    
    public event System.Action<Task> OnTaskCompleted;

    public virtual void Initialize(GameObject taskObject)
    {
        this.taskObject = taskObject;
        taskCompleted = false;

        if (this.taskObject == null)
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
