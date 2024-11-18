using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskZone : MonoBehaviour
{
    private ITask task; // Ссылка на задание
    private GameObject entityInZone; // Ссылка на объект (игрок или бот) в зоне

    private void Start()
    {
        // Получаем компонент задания
        task = GetComponent<ITask>();
        if (task == null)
        {
            Debug.LogError("Задание (ITask) не найдено на объекте " + gameObject.name);
        }
    }

    private void Update()
    {
        // Если объект в зоне — проверяем взаимодействие
        if (entityInZone != null)
        {
            // Для игрока — проверяем нажатие клавиши
            if (entityInZone.CompareTag("Hider") && Input.GetKeyDown(KeyCode.E))
            {
                PerformTask(entityInZone);
            }

            // Для бота выполнение может быть инициировано в другом месте (например, в скрипте ИИ)
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
            Debug.Log("Задание уже выполнено или не задано.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hider") || other.CompareTag("Bot"))
        {
            entityInZone = other.gameObject;
            Debug.Log($"{entityInZone.name} вошел в зону задания: {gameObject.name}");

            // Для бота можно автоматически выполнить задание
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
            Debug.Log($"{entityInZone.name} вышел из зоны задания: {gameObject.name}");
            entityInZone = null;
        }
    }
}