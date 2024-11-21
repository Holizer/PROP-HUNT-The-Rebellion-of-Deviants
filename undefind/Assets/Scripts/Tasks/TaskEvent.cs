using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskEvent : MonoBehaviour
{
    public delegate void TaskCompletedHandler(GameObject performer, ITask task);
   
    public static event TaskCompletedHandler OnTaskCompleted;

    public static void CompleteTask(GameObject performer, ITask task)
    {
        OnTaskCompleted?.Invoke(performer, task);
    }
}
