using UnityEngine;
public class TaskZone : MonoBehaviour
{
    [SerializeField] private ITask task;

    private void Start()
    {
        task = GetComponentInParent<ITask>();
        if (task == null)
        {
            Debug.LogError("«адание не прив€зано к этой зоне.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider"))
        {
            Debug.Log($"{other.name} вошел в зону задани€!");
            task.PerformTask(other.gameObject);
        }
    }
}
