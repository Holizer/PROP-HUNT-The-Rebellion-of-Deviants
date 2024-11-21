using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskFactory : MonoBehaviour
{
    public ITask CreateTask(GameObject taskObject, string taskType)
    {
        switch (taskType)
        {
            case "PoisonWater":
                return taskObject.AddComponent<PoisonWaterTask>();
            default:
                Debug.LogError($"Неизвестный тип задания: {taskType}");
                return null;
        }
    }
}
