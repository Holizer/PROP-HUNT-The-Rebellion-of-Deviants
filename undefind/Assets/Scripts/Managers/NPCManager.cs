using System;
using UnityEngine;

public enum NPCModel
{
    King,
    Queen,

    M1_RichCitizzen,
    M2_RichCitizzen,

    W1_RichCitizzen,
    W2_RichCitizzen,

    M1_Peasant,
    M2_Peasant,
    M3_Peasant,

    W1_Peasant,
    W2_Peasant,
    W3_Peasant,
    
    M_Dweller,
    W_Dweller
}

public class NPCManager : MonoBehaviour
{
    [Header("NPC Префабы")]

    private GameObject selectedNPCPrefab;

    public void GenerateRandomNPC()
    {
        NPCModel randomNPC = (NPCModel)UnityEngine.Random.Range(0, Enum.GetValues(typeof(NPCModel)).Length);

        switch (randomNPC)
        {
            case NPCModel.King:
                break;
        }

        if (selectedNPCPrefab != null)
        {
            Instantiate(selectedNPCPrefab, transform.position, Quaternion.identity);
            Debug.Log($"Сгенерирован NPC: {randomNPC}");
        }
        else
        {
            Debug.LogError("Префаб NPC не найден.");
        }
    }
}
