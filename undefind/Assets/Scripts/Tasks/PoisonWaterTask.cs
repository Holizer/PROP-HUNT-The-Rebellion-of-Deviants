using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWaterTask : MonoBehaviour, ITask
{
    private bool taskCompleted;
    private GameObject taskObject;

    public void Initialize(GameObject targetObject)
    {
        taskObject = targetObject;
    }

    public void PerformTask(GameObject performer)
    {
        if (taskCompleted) return;

        if (performer.CompareTag("Hider"))
        {
            taskCompleted = true;
            Debug.Log($"{performer.name} ������� ����!");
        }
        else
        {
            Debug.Log("� ����� ������� ��� ���� �� ���������� �������.");
        }
    }

    public bool IsCompleted() => taskCompleted;
    public TaskType GetTaskType() => TaskType.Unique;
}