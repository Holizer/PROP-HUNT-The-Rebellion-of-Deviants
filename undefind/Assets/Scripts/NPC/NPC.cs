using UnityEngine;
public abstract class NPC : MonoBehaviour
{
    [Header("ќсновные параметры NPC")]
    [SerializeField] private string npcName;
    [SerializeField] private GameObject modelPrefab;

    public string NpcName => npcName;
    public GameObject ModelPrefab => modelPrefab;

    public virtual void PerformBehavior()
    {
        Debug.Log($"NPC {npcName} выполн€ет базовое поведение.");
    }

    public void SetName(string name)
    {
        npcName = name;
    }

    public void SetModelPrefab(GameObject prefab)
    {
        modelPrefab = prefab;
    }
}