using UnityEngine;
public class TaskZone : MonoBehaviour
{
    [SerializeField] private ITask task;

    private void Start()
    {
        task = GetComponentInParent<ITask>();
        if (task == null)
        {
            Debug.LogError("������� �� ��������� � ���� ����.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider"))
        {
            Debug.Log($"{other.name} ����� � ���� �������!");
            task.PerformTask(other.gameObject);
        }
    }
}
