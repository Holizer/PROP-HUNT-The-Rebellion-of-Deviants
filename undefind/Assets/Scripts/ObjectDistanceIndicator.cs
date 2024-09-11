using UnityEngine;

public class ObjectDistanceIndicator : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject replacementIndicatorPrefab;
    public float maxDistance = 10f;
    public LayerMask taskItemLayerMask;

    private Transform canvasTransform;
    private GameObject[] taskItems;

    void Start()
    {
        FindTaskItems();
        CreateIndicators();
    }
    void FindTaskItems()
    {
        taskItems = GameObject.FindObjectsOfType<GameObject>();
        taskItems = System.Array.FindAll(taskItems, obj => (taskItemLayerMask & (1 << obj.layer)) != 0);

        if (taskItems.Length == 0)
        {
            Debug.LogWarning("Не найдено объектов на слое TaskItem.");
        }
    }

    void Update()
    {
        if (canvasTransform == null)
        {
            Debug.LogError("Canvas не найден. Обновление индикаторов невозможно.");
            return;
        }

        foreach (var taskItem in taskItems)
        {
            if (taskItem == null) continue;

            float distance = Vector3.Distance(mainCamera.transform.position, taskItem.transform.position);
            Transform existingIndicator = GetExistingIndicator(taskItem);

            if (distance > maxDistance)
            {
                if (existingIndicator == null)
                {
                    Debug.LogWarning($"Не найден индикатор для объекта {taskItem.name}. Проверьте корректность логики.");
                }
                else
                {
                    // Обновляем позицию существующего индикатора
                    UpdateIndicatorPosition(existingIndicator, taskItem);
                }
            }
            else
            {
                if (existingIndicator != null)
                {
                    // Удаляем индикатор, если он есть и объект в пределах видимости
                    existingIndicator.gameObject.SetActive(false);
                }
            }
        }
    }

    void CreateIndicators()
    {
        foreach (var taskItem in taskItems)
        {
            if (taskItem == null) continue;

            // Создаем UI индикатор и привязываем его к объекту
            GameObject indicator = Instantiate(replacementIndicatorPrefab, Vector3.zero, Quaternion.identity, canvasTransform);
            indicator.name = "ReplacementIndicator_" + taskItem.GetInstanceID();
            RectTransform rectTransform = indicator.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = WorldToScreenPosition(taskItem.transform.position);
            // Делаем индикатор активным или скрытым по умолчанию
            indicator.SetActive(false);
        }
    }

    Transform GetExistingIndicator(GameObject taskItem)
    {
        string indicatorName = "ReplacementIndicator_" + taskItem.GetInstanceID();
        foreach (Transform child in canvasTransform)
        {
            if (child.gameObject.name == indicatorName)
            {
                return child;
            }
        }
        return null;
    }

    void UpdateIndicatorPosition(Transform indicatorTransform, GameObject taskItem)
    {
        RectTransform rectTransform = indicatorTransform.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = WorldToScreenPosition(taskItem.transform.position);
            indicatorTransform.gameObject.SetActive(true); // Убедитесь, что индикатор активен
        }
    }

    Vector2 WorldToScreenPosition(Vector3 worldPosition)
    {
        return mainCamera.WorldToScreenPoint(worldPosition);
    }
}
