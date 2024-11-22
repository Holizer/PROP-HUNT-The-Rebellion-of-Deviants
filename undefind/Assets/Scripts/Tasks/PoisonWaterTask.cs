using UnityEngine;

public class PoisonWaterTask : Task
{
    public override string TaskName => "Отравить воду";
    public override void Initialize(GameObject targetObject)
    {
        base.Initialize(targetObject ?? gameObject);
        taskType = TaskType.Unique; 
    }

    public override void PerformTask(GameObject performer)
    {
        base.PerformTask(performer);

        if (performer.CompareTag("Hider"))
        {
            base.CompleteTask(); 
            Debug.Log($"{performer.name} отравил воду!");
        }
        else
        {
            Debug.Log($"{performer.name} не может выполнить это задание.");
        }
    }
}
