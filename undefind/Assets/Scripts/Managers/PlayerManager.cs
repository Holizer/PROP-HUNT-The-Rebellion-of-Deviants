using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [Header("Префабы")]
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
            Debug.LogError("Роль для игрока не найдена!");
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
            currentPlayer = PhotonNetwork.Instantiate(hiderPrefab.name, spawnPosition, Quaternion.identity);
        }
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