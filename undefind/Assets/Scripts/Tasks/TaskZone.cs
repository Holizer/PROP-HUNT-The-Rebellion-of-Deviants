using UnityEngine;

public class TaskZone : MonoBehaviour
{
    private Task task;
    public event System.Action<Task> OnPlayerEnteredZone;
    public event System.Action<Task> OnPlayerLeftZone;
    private void Start()
    {
        task = GetComponentInParent<Task>();
        if (task == null)
        {
            Debug.LogError("Задание не привязано к этой зоне.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider") && !task.IsCompleted())
        {
            OnPlayerEnteredZone?.Invoke(task);
            TaskManager.Instance.SetPerformer(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Hider") && !task.IsCompleted())
        {
            OnPlayerLeftZone?.Invoke(task);
        }
    }
}