using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class TaskVisibilityController : MonoBehaviour
{
    public Transform player;
    private PhotonView view;
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
                Debug.LogError($"TaskContainer не найден!");
            }
        }
    }

    public void MakeTaskInvisibleForHunter(Transform task)
    {
        DestroyTaskZoneFromTask(task);
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
            Debug.LogWarning($"TaskZone не найден для {task.name}");
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
            Debug.LogWarning($"У {parent.name} нет дочерних объектов");
        }
    }
}
