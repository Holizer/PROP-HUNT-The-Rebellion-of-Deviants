using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("Модели крестьян")]
    public List<GameObject> peasantModels;

    private void Start()
    {
        NPCFactory peasantFactory = new PeasantFactory();
        if (peasantModels == null || peasantModels.Count == 0)
        {
            Debug.LogError("Список моделей крестьян не заполнен!");
            return;
        }

        NPC peasant = peasantFactory.CreateNPC("Иван", peasantModels);
        peasant.PerformBehavior();
    }
}
