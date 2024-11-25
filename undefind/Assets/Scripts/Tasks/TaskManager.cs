using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [Header("Лист заданий")]
    public TaskList taskList;
    private List<Task> tasks = new List<Task>();

    [Header("Персонаж игрока или бота")]
    public GameObject performer;

    [Header("UI Подсказака управления")]
    public GameObject taskHintUI;
    private Task currentTask;

    private void Start()
    {
        if (taskList == null)
        {
            Debug.LogError("TaskList не найден в сцене!");
        }
        InitializeTasks();
    }

    private void InitializeTasks()
    {
        GameObject taskContainerObject = GameObject.Find("TaskContainer");

        if (taskContainerObject != null)
        {
            Transform taskContainer = taskContainerObject.transform;

            tasks.Clear();
            foreach (Transform child in taskContainer)
            {
                Task task = child.GetComponent<Task>();
                if (task != null)
                {
                    tasks.Add(task);
                    task.Initialize(gameObject);

                    SetTaskEventHandlers(task);

                    SetTaskZoneEventHandlers(task);
                }
            }

            if (taskList != null)
            {
                taskList.UpdateTaskUI(tasks);
            }
            else
            {
                Debug.LogError("TaskList не привязан к TaskManager!");
            }
        }
        else
        {
            Debug.LogError("TaskContainer не найден на сцене! Убедитесь, что объект существует и назван корректно.");
        }
    }

    #region TaskZoneLogic

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void SetTaskEventHandlers(Task task)
    {
        task.OnTaskCompleted += HandleTaskCompleted;
    }
    private void SetTaskZoneEventHandlers(Task task)
    {
        TaskZone taskZone = task.GetComponentInChildren<TaskZone>();
        if (taskZone != null)
        {
            taskZone.OnPlayerEnteredZone += HandlePlayerEnteredZone;
            taskZone.OnPlayerLeftZone += HandlePlayerLeftZone;
        }
    }

    private void HandleTaskCompleted(Task completedTask)
    {
        taskList.UpdateTaskUI(tasks);
    }
    private void HandlePlayerLeftZone(Task task)
    {
        currentTask = null;
        if (taskHintUI != null)
        {
            taskHintUI.SetActive(false);
        }
    }
    private void HandlePlayerEnteredZone(Task task)
    {
        currentTask = task;
        if (taskHintUI != null)
        {
            taskHintUI.SetActive(true);
        }
    }

    private void TurnPlayerToTask(Task task)
    {
        if (performer != null)
        {
            Vector3 targetPosition = task.transform.position;
            Vector3 direction = targetPosition - performer.transform.position;
            direction.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            performer.transform.rotation = targetRotation;
        }
    }

    #endregion

    #region PerfomTaskLogic

    private void Update()
    {
        if (currentTask != null && taskHintUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            PerformTask(currentTask);
        }
    }

    private void PerformTask(Task task)
    {
        if (performer == null)
        {
            Debug.LogError("Performer не установлен. Убедитесь, что объект performer задан.");
            return;
        }

        TurnPlayerToTask(task);
        taskHintUI?.SetActive(false);

        PlayTaskAnimation(performer, task);

        task.PerformTask(performer);
    }


    private void PlayTaskAnimation(GameObject performer, Task task)
    {
        Animator animator = performer.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"Аниматор не найден у объекта {performer.name}. Анимация пропущена.");
            return;
        }

        string animationParameter = GetAnimationParameterForTask(task);
        Debug.Log("animationParameter " + animationParameter);
        if (!string.IsNullOrEmpty(animationParameter))
        {
            animator.SetBool(animationParameter, true);

            StartCoroutine(WaitForAnimationToComplete(animator, animationParameter));
        }
    }

    private IEnumerator WaitForAnimationToComplete(Animator animator, string animationParameter)
    {
        if (performer.CompareTag("Hider"))
        {
            HandleHider(performer, false);
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        yield return new WaitForSeconds(0.1f);

        if (performer.CompareTag("Hider"))
        {
            HandleHider(performer, true);
        }

        animator.SetBool(animationParameter, false);
    }

    private void HandleHider(GameObject hider, bool isEnabled)
    {
        if (hider == null)
        {
            Debug.LogWarning("Hider не найден!");
            return;
        }

        // Отключаем/включаем движение и анимацию
        ComponentUtils.ToggleComponent<HiderMovement>(hider, isEnabled);
        ComponentUtils.ToggleComponent<HiderAnimation>(hider, isEnabled);
    }

    private string GetAnimationParameterForTask(Task task)
    {
        switch (task.GetTaskType())
        {
            case TaskType.Unique:
                return "isTalking";
            case TaskType.Common:
                return "isInteracting";
            default:
                Debug.LogWarning($"Неизвестный тип задания: {task.GetTaskType()}");
                return string.Empty;
        }
    }

    #endregion
}