using UnityEngine;

public class TaskZone : MonoBehaviour
{
    [SerializeField] private Task task;

    private void Start()
    {
        task = GetComponentInParent<Task>();
        if (task == null)
        {
            Debug.LogError("«адание не прив€зано к этой зоне.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider") && !task.IsCompleted())
        {
            Debug.Log($"{other.name} вошел в зону задани€!");
            task.PerformTask(other.gameObject);
        }
    }
}