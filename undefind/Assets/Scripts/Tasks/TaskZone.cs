using UnityEngine;

public class TaskZone : MonoBehaviour
{
    [SerializeField] private Task task;

    private void Start()
    {
        task = GetComponentInParent<Task>();
        if (task == null)
        {
            Debug.LogError("������� �� ��������� � ���� ����.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider") && !task.IsCompleted())
        {
            Debug.Log($"{other.name} ����� � ���� �������!");
            task.PerformTask(other.gameObject);
        }
    }
}