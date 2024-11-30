using UnityEngine;
using Photon.Pun;

public class LocalPlayerValidator : MonoBehaviour
{
    [Tooltip("Отключить указанные компоненты для нелокального игрока.")]
    public MonoBehaviour[] componentsToDisable;

    [Tooltip("Отключить указанные объекты для нелокального игрока.")]
    public GameObject[] objectsToDisable;

    void Awake()
    {
        if (!TryGetComponent<PhotonView>(out var photonView) || !photonView.IsMine)
        {
            foreach (var component in componentsToDisable)
            {
                component.enabled = false;
            }

            foreach (var obj in objectsToDisable)
            {
                obj.SetActive(false);
            }

            enabled = false;
        }
    }

    void Start()
    {
        if(transform.tag == "Hunter")
        {
            GameObject taskContainerObject = GameObject.Find("TaskContainer");

            if (taskContainerObject != null)
            {
                Transform taskContainer = taskContainerObject.transform;
                
                foreach (Transform child in taskContainer)
                {
                    DisableTaskVisibility(child);
                }
            }
            else
            {
                Debug.LogError("TaskContainer не найден на сцене! Убедитесь, что объект существует и назван корректно.");
            }
        }
    }

    private void DisableTaskVisibility(Transform taskTransform)
    {
        Task task = taskTransform.GetComponent<Task>();

        if (task != null)
        {
            taskTransform.gameObject.layer = LayerMask.NameToLayer("Default");

            Transform taskZone = taskTransform.Find("TaskZone");

            if (taskZone != null)
            {
                UpdateLayerRecursively(taskZone, "TaskItem", "Default");
                Destroy(taskZone.gameObject);
            }
        }
        else
        {
            Debug.LogWarning($"Объект {taskTransform.name} не имеет компонента Task!");
        }
    }

    void UpdateLayerRecursively(Transform parent, string currentLayer, string newLayer)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer(currentLayer))
            {
                child.gameObject.layer = LayerMask.NameToLayer(newLayer);
            }

            if (child.childCount > 0)
            {
                UpdateLayerRecursively(child, currentLayer, newLayer);
            }
        }
    }
}