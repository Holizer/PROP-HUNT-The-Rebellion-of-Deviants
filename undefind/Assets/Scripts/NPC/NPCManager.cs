using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    [Header("Модели крестьян")]
    public List<GameObject> peasantModels;

    [Header("NavMesh Surface")]
    public NavMeshSurface navMeshSurface;
    
    [Header("Точки интереса")]
    public List<Transform> pointsOfInterest;
    public static List<GameObject> NPCInstances { get; private set; } = new List<GameObject>();
    private GameObject reservedNPCModel;
    void Awake()
    {
        ReserveNPCForHider();
        SpawnAllNPCs();
    }
    private void ReserveNPCForHider()
    {
        if (peasantModels == null || peasantModels.Count == 0)
        {
            Debug.LogError("Список моделей крестьян не заполнен!");
            return;
        }

        int randomIndex = Random.Range(0, peasantModels.Count);
        reservedNPCModel = peasantModels[randomIndex];
        peasantModels.RemoveAt(randomIndex);
    }

    public GameObject GetReservedNPCModel()
    {
        return reservedNPCModel;
    }

    private void SpawnAllNPCs()
    {
        if (peasantModels == null || peasantModels.Count == 0)
        {
            Debug.LogError("Список моделей крестьян не заполнен!");
            return;
        }

        foreach (GameObject model in peasantModels)
        {
            GameObject npc = Instantiate(model);
            npc.name = model.name;

            Vector3 spawnPosition = GetRandomNavMeshPosition();
            npc.transform.position = spawnPosition;

            NavMeshAgent agent = npc.AddComponent<NavMeshAgent>();
            NPCInstances.Add(npc);

            BotAI botAI = npc.AddComponent<BotAI>();
            botAI.pointsOfInterest = pointsOfInterest;

            CapsuleCollider capsule = npc.AddComponent<CapsuleCollider>();
            capsule.height = 2f;
            capsule.center = new Vector3(0, 1f, 0);
            capsule.radius = 0.35f;
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position; 
        }

        return randomPosition; 
    }
}
