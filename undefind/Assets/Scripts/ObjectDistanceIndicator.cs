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
            Debug.LogWarning("�� ������� �������� �� ���� TaskItem.");
        }
    }

    void Update()
    {
        if (canvasTransform == null)
        {
            Debug.LogError("Canvas �� ������. ���������� ����������� ����������.");
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
                    Debug.LogWarning($"�� ������ ��������� ��� ������� {taskItem.name}. ��������� ������������ ������.");
                }
                else
                {
                    // ��������� ������� ������������� ����������
                    UpdateIndicatorPosition(existingIndicator, taskItem);
                }
            }
            else
            {
                if (existingIndicator != null)
                {
                    // ������� ���������, ���� �� ���� � ������ � �������� ���������
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

            // ������� UI ��������� � ����������� ��� � �������
            GameObject indicator = Instantiate(replacementIndicatorPrefab, Vector3.zero, Quaternion.identity, canvasTransform);
            indicator.name = "ReplacementIndicator_" + taskItem.GetInstanceID();
            RectTransform rectTransform = indicator.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = WorldToScreenPosition(taskItem.transform.position);
            // ������ ��������� �������� ��� ������� �� ���������
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
            indicatorTransform.gameObject.SetActive(true); // ���������, ��� ��������� �������
        }
    }

    Vector2 WorldToScreenPosition(Vector3 worldPosition)
    {
        return mainCamera.WorldToScreenPoint(worldPosition);
    }
}
