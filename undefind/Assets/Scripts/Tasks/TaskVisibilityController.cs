using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class TaskVisibilityController : MonoBehaviour
{
    public Transform player;
    public string outlineMaterialName = "Outline";
    private PhotonView view;

    void Awake()
    {
    }

    void Start()
    {
        view = player.GetComponent<PhotonView>();
        if (view.IsMine)
        {
            GameObject taskContainerObject = GameObject.Find("TaskContainer");
            if (taskContainerObject != null)
            {
                Transform taskContainer = taskContainerObject.transform;

                foreach (Transform task in taskContainer)
                {
                    MakeTaskInvisibleForHunter(task);
                }
            }
            else
            {
                Debug.LogError($"TaskContainer �� ������!");
            }
        }
        else
        {
            Debug.Log("�������� �� �����������, ��� ��� ����� �� �������� ��������� ��� ��� �� ��������� ������.");
        }
    }

    public void MakeTaskInvisibleForHunter(Transform task)
    {
        DestroyTaskZoneFromTask(task);

        RemoveMaterialFromFirstChild(task);

        SetLayerForFirstChild(task, 0);
    }
    public void DestroyTaskZoneFromTask(Transform task)
    {
        Transform taskZone = task.Find("TaskZone");
        if (taskZone != null)
        {
            Destroy(taskZone.gameObject);
        }
        else
        {
            Debug.LogWarning($"TaskZone �� ������ ��� {task.name}");
        }
    }

    public void RemoveMaterialFromFirstChild(Transform parent)
    {
        if (parent.childCount > 0)
        {
            Transform firstChild = parent.GetChild(0);
            Renderer renderer = firstChild.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.materials = new Material[0];
            }
            else
            {
                Debug.LogWarning($"Renderer �� ������ � {firstChild.name}");
            }
        }
        else
        {
            Debug.LogWarning($"� {parent.name} ��� �������� ��������");
        }
    }

    public void SetLayerForFirstChild(Transform parent, int layer)
    {
        if (parent.childCount > 0)
        {
            Transform firstChild = parent.GetChild(0);
            firstChild.gameObject.layer = layer;
        }
        else
        {
            Debug.LogWarning($"� {parent.name} ��� �������� ��������");
        }
    }
}
