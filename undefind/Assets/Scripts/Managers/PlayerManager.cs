using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [Header("Префабы")]
    public GameObject hunterPrefab;
    public GameObject hiderPrefab;

    public GameObject spawnArea;
    private GameObject currentPlayer;

    void Start()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out object roleObj))
        {
            string roleString = roleObj.ToString();
            PlayerRole role = (PlayerRole)System.Enum.Parse(typeof(PlayerRole), roleString);

            Vector3 spawnPosition = GetSpawnPosition();
            SpawnPlayer(role, spawnPosition);
        }
        else
        {
            Debug.LogError("Роль для игрока не найдена!");
        }
    }

    private void SpawnPlayer(PlayerRole role, Vector3 spawnPosition)
    {
        if (role == PlayerRole.Hunter)
        {
            currentPlayer = PhotonNetwork.Instantiate(hunterPrefab.name, spawnPosition, Quaternion.identity);
            currentPlayer.GetComponent<HunterSetup>().SetupLocalPlayer();
        }
        else
        {
            currentPlayer = PhotonNetwork.Instantiate(hiderPrefab.name, spawnPosition, Quaternion.identity);
            currentPlayer.GetComponent<HiderSetup>().SetupLocalPlayer();
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Bounds bounds = spawnArea.GetComponent<BoxCollider>().bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }
}
