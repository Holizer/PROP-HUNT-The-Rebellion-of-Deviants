using UnityEngine;
using Photon.Pun;
public class PlayerManager : MonoBehaviour
{
    [Header("�������")]
    public GameObject hunterPrefab;
    public GameObject hiderPrefab;

    public GameObject hunterSpawnArea;
    public GameObject hiderSpawnArea;
    private GameObject currentPlayer;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object roleObj))
        {
            string roleString = roleObj.ToString();
            PlayerRole role = (PlayerRole)System.Enum.Parse(typeof(PlayerRole), roleString);
            SpawnPlayer(role);
        }
        else
        {
            Debug.LogError("���� ��� ������ �� �������!");
        }
    }

    private void SpawnPlayer(PlayerRole role)
    {
        if (role == PlayerRole.Hunter)
        {
            Vector3 spawnPosition = GetSpawnPosition(hunterSpawnArea);
            currentPlayer = PhotonNetwork.Instantiate(hunterPrefab.name, spawnPosition, Quaternion.identity);
        }
        else
        {
            Vector3 spawnPosition = GetSpawnPosition(hiderSpawnArea);
            AssignNPCToHider(spawnPosition);
        }
    }

    private void AssignNPCToHider(Vector3 spawnPosition)
    {
        if (NPCManager.NPCInstances == null || NPCManager.NPCInstances.Count == 0)
        {
            Debug.LogError("��� ��������� NPC ��� ���������� Hider!");
            return;
        }

        currentPlayer = PhotonNetwork.Instantiate(hiderPrefab.name, spawnPosition, Quaternion.identity);
        
        Transform modelContainer = currentPlayer.transform.Find("Model");
        if (modelContainer == null)
        {
            Debug.LogError("Model �� ������ ������ HiderPrefab!");
            return;
        }

        int randomIndex = Random.Range(0, NPCManager.NPCInstances.Count - 1);
        GameObject selectedNPC = NPCManager.NPCInstances[randomIndex];
        
        GameObject npcModel = Instantiate(selectedNPC, modelContainer.position, modelContainer.rotation, modelContainer);
        npcModel.name = selectedNPC.name;

        Debug.Log($"Hider ������� ������ NPC: {currentPlayer.name}");
    }

    private Vector3 GetSpawnPosition(GameObject spawnArea)
    {
        Bounds bounds = spawnArea.GetComponent<BoxCollider>().bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
}
