using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    [Header("Префабы")]
    public GameObject hunterPrefab;
    public GameObject hiderPrefab;

    public GameObject spawnArea;
    private GameObject currentPlayer;
    void Start()
    {
        PlayerRole selectedRole = ChooseRandomRole();
        Vector3 spawnPosition = GetSpawnPosition();
        SpawnPlayer(PlayerRole.Hunter, spawnPosition);
    }
    private PlayerRole ChooseRandomRole()
    {
        return (PlayerRole)Random.Range(0, System.Enum.GetValues(typeof(PlayerRole)).Length);
    }
    private void SpawnPlayer(PlayerRole role, Vector3 spawnPosition)
    {
        currentPlayer = PhotonNetwork.Instantiate(hunterPrefab.name, spawnPosition, Quaternion.identity);
        currentPlayer.GetComponent<PlayerSetup>().IsLocalPlayer();
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