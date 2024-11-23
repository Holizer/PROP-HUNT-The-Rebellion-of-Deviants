using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask
{
    void Initialize(GameObject taskObject);
    void PerformTask(GameObject performer);
    bool IsCompleted();
    TaskType GetTaskType();
}

public enum TaskType
{
    Common,
    Unique
}