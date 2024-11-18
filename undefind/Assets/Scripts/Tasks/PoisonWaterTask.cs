using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWaterTask : MonoBehaviour, ITask
{
    [SerializeField] private bool completed = false;
    [SerializeField] private GameObject targetObject;

    public void Initialize(GameObject targetObject)
    {
        this.targetObject = targetObject;
    }

    public void PerformTask(GameObject performer)
    {
        if (completed)
        {
            Debug.Log("������� ��� ���������.");
            return;
        }

        Debug.Log($"{performer.name} ��������� ������� '���������� ����'.");
        StartCoroutine(CompleteTask());
    }

    private IEnumerator CompleteTask()
    {
        yield return new WaitForSeconds(3f);
        completed = true;
        Debug.Log("���� ���������!");
    }

    public bool IsCompleted()
    {
        return completed;
    }
}

