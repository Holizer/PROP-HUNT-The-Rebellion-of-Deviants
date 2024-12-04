using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    [Header("������ ��������")]
    public List<GameObject> peasantModels;
    public static List<GameObject> NPCInstances { get; private set; } = new List<GameObject>();

    void Awake()
    {
        SpawnAllNPCs();
    }

    private void SpawnAllNPCs()
    {
        if (peasantModels == null || peasantModels.Count == 0)
        {
            Debug.LogError("������ ������� �������� �� ��������!");
            return;
        }

        foreach (GameObject model in peasantModels)
        {
            GameObject npc = Instantiate(model);
            npc.name = model.name;
            // ���������� ���������� NavMeshAgent
            NavMeshAgent agent = npc.AddComponent<NavMeshAgent>();
            NPCInstances.Add(npc);
            Debug.Log($"������ NPC: {npc.name}");

            // ������������� ����� ��������� ��������, ������ � ������ ��������� ������
            agent.speed = 3.5f;
            agent.angularSpeed = 120f;
        }
    }
}
