using UnityEngine;

public class StealTask : Task
{
    public override string taskName => "������� ������";
    public override string animationParameter => "isPicking";
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

            DestroyTaskAndZone();
            
            Debug.Log($"{performer.name} ������� ����� �������!");
        }
    }
    private void DestroyTaskAndZone()
    {
        Destroy(gameObject);

        TaskZone taskZone = GetComponentInChildren<TaskZone>();
        if (taskZone != null)
        {
            Destroy(taskZone.gameObject);
        }
    }
}
