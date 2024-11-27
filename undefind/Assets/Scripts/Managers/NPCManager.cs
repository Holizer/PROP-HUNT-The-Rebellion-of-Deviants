using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("������ ��������")]
    public List<GameObject> peasantModels;

    private void Start()
    {
        NPCFactory peasantFactory = new PeasantFactory();
        if (peasantModels == null || peasantModels.Count == 0)
        {
            Debug.LogError("������ ������� �������� �� ��������!");
            return;
        }

        NPC peasant = peasantFactory.CreateNPC("����", peasantModels);
        peasant.PerformBehavior();
    }
}
