using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    [Header("Модели крестьян")]
    public List<GameObject> peasantModels;
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
            
            NavMeshAgent agent = npc.AddComponent<NavMeshAgent>();
            NPCInstances.Add(npc);

            npc.AddComponent<BotAI>();

            CapsuleCollider capsule = npc.AddComponent<CapsuleCollider>();
            capsule.height = 2f;
            capsule.center = new Vector3(0, 1f, 0);
            capsule.radius = 0.35f;
        }
    }
}
