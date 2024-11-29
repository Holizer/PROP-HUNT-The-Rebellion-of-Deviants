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
    
    public static TaskManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

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

    public void SetPerformer(GameObject newPerformer)
    {
        performer = newPerformer;

        if (performer.CompareTag("Hider"))
        {
            Debug.Log("Performer - игрок");
        }
        else if (performer.CompareTag("NPC"))
        {
            Debug.Log("Performer - бот");
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
    }

    private void PlayTaskAnimation(GameObject performer, Task task)
    {
        Animator animator = performer.GetComponentInChildren<Animator>();
        if (animator == null)
        {
            //Debug.LogWarning($"Аниматор не найден у объекта {performer.name}. Анимация пропущена.");
            return;
        }

        string animationParameter = GetAnimationParameterForTask(task);
        if (!string.IsNullOrEmpty(animationParameter))
        {
            //Debug.Log($"Попытка включить анимацию '{animationParameter}' для задачи '{task.GetTaskType()}'.");

            animator.SetBool(animationParameter, true);

            StartCoroutine(WaitForAnimationToComplete(animator, animationParameter, task));
        }
        else
        {
            Debug.LogWarning("Анимация не была найдена. Параметр пуст.");
        }
    }

    private IEnumerator WaitForAnimationToComplete(Animator animator, string animationParameter, Task task)
    {
        if (performer.CompareTag("Hider"))
        {
            HandleHider(performer, false);
        }
        
        yield return new WaitForSeconds(0.5f);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float animationLength = stateInfo.length;
        float adjustedLength = animationLength / animator.speed;
        //Debug.Log($"Длительность анимации: {adjustedLength} секунд.");

        float startTime = Time.time;
        while (Time.time - startTime < adjustedLength)
        {
            yield return null; 
        }

        animator.SetBool(animationParameter, false);
        
        if (performer.CompareTag("Hider"))
        {
            HandleHider(performer, true);
        }

        task.PerformTask(performer);
    }

    private void HandleHider(GameObject hider, bool isEnabled)
    {
        if (hider == null)
        {
            Debug.LogWarning("Hider не найден!");
            return;
        }

        GameObject scriptsContainer = hider.transform.Find("Scripts")?.gameObject;
        
        ComponentUtils.ToggleComponent<HiderMovement>(scriptsContainer, isEnabled);
        ComponentUtils.ToggleComponent<HiderAnimation>(scriptsContainer, isEnabled);
    }

    private string GetAnimationParameterForTask(Task task)
    {
        string animationParameter = task.animationParameter;

        if (!string.IsNullOrEmpty(animationParameter))
        {
            return animationParameter;
        }

        Debug.LogWarning($"Неизвестный тип задания: {task.GetTaskType()}");
        return string.Empty;
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
}
