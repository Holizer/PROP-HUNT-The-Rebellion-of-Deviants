using UnityEngine;

public class PoisonWaterTask : Task
{
    public override string taskName => "Отравить воду";

    public override void Initialize(GameObject taskObject)
    {
        base.Initialize(gameObject);
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
