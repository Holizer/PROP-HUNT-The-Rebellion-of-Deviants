using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObject : MonoBehaviour
{
    public ITask task;

    private void Start()
    {
        task = GetComponent<PoisonWaterTask>();
        if (task != null)
        {
            task.Initialize(this.gameObject);
        }
    }

    public void Interact(GameObject performer)
    {
        if (task != null && !task.IsCompleted())
        {
            task.PerformTask(performer);
        }
        else
        {
            Debug.Log("Задание уже выполнено или не задано.");
        }
    }
}
