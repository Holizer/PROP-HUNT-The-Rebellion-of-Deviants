using System.Collections.Generic;
using UnityEngine;

public class PeasantNPC : NPC
{
    public override void PerformBehavior()
    {
        Debug.Log($"���������� {NpcName} �������� �� ����.");
    }

    public void AssignRandomModel(List<GameObject> models)
    {
        if (models == null || models.Count == 0)
        {
            Debug.LogError("������ ������� �������� ����.");
            return;
        }

        int randomIndex = Random.Range(0, models.Count);
        GameObject selectedModel = models[randomIndex];
        GameObject modelInstance = Instantiate(selectedModel);
        modelInstance.transform.SetParent(transform);
        SetModelPrefab(modelInstance);
    }
}
