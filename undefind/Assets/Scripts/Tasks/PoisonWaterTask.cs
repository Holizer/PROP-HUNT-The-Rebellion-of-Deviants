using UnityEngine;

public class PoisonWaterTask : Task
{
    public override string taskName => "�������� ����";

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
            Debug.Log($"{performer.name} ������� ����!");
        }
        else
        {
            Debug.Log($"{performer.name} �� ����� ��������� ��� �������.");
        }
    }
}
