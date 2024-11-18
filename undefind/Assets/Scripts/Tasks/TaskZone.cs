using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskZone : MonoBehaviour
{
    private ITask task; // ������ �� �������
    private GameObject entityInZone; // ������ �� ������ (����� ��� ���) � ����

    private void Start()
    {
        // �������� ��������� �������
        task = GetComponent<ITask>();
        if (task == null)
        {
            Debug.LogError("������� (ITask) �� ������� �� ������� " + gameObject.name);
        }
    }

    private void Update()
    {
        // ���� ������ � ���� � ��������� ��������������
        if (entityInZone != null)
        {
            // ��� ������ � ��������� ������� �������
            if (entityInZone.CompareTag("Hider") && Input.GetKeyDown(KeyCode.E))
            {
                PerformTask(entityInZone);
            }

            // ��� ���� ���������� ����� ���� ������������ � ������ ����� (��������, � ������� ��)
        }
    }

    private void PerformTask(GameObject performer)
    {
        if (task != null && !task.IsCompleted())
        {
            task.PerformTask(performer);
        }
        else
        {
            Debug.Log("������� ��� ��������� ��� �� ������.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider") || other.CompareTag("Bot"))
        {
            entityInZone = other.gameObject;
            Debug.Log($"{entityInZone.name} ����� � ���� �������: {gameObject.name}");

            // ��� ���� ����� ������������� ��������� �������
            if (other.CompareTag("Bot"))
            {
                PerformTask(entityInZone);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == entityInZone)
        {
            Debug.Log($"{entityInZone.name} ����� �� ���� �������: {gameObject.name}");
            entityInZone = null;
        }
    }
}