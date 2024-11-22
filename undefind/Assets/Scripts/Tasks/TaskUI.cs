using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public TextMeshProUGUI taskNameText;
    public TextMeshProUGUI taskStatusText;
    public void Initialize(string taskName, string taskStatus)
    {
        if (taskNameText != null)
        {
            taskNameText.text = taskName;
        }
        if (taskStatusText != null)
        {
            taskStatusText.text = taskStatus;
        }
    }
}