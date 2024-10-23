using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviour
{
    [Header("Префабы")]
    public GameObject hunterPrefab;
    //public GameObject hiderPrefab;

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
        //currentPlayerModel = InstantiatePlayerModel(role, currentPlayer.transform);
        currentPlayer.GetComponent<PlayerSetup>().IsLocalPlayer();
        //InitializePlayerRoleScripts(role);
    }
    //private GameObject InstantiatePlayerModel(PlayerRole role, Transform parent)
    //{
    //    GameObject modelPrefab = role == PlayerRole.Hunter ? hunterModelPrefab : hiderModelPrefab;
    //    GameObject model = Instantiate(modelPrefab, parent);

    //    model.transform.localPosition = Vector3.zero;
    //    model.transform.localRotation = Quaternion.identity;

    //    return model;
    //}
    //private void InitializePlayerRoleScripts(PlayerRole role)
    //{
    //    Player playerScript = currentPlayer.GetComponent<Player>();
    //    if (playerScript != null)
    //    {
    //        playerScript.SetRole(role);
    //        switch (role)
    //        {
    //            case PlayerRole.Hunter:
    //            {
    //                Hunter hunter = currentPlayer.AddComponent<Hunter>();
    //                if      (hunter != null) hunter.Initialize(currentPlayer, currentPlayerModel);
    //                else    Debug.LogError("Не удалось создать Hunter компонент для игрока!");
    //                break;
    //            }

    //            case PlayerRole.Hider:
    //            {
    //                Hider hider = currentPlayer.AddComponent<Hider>();
    //                if      (hider != null) hider.Initialize(currentPlayer, currentPlayerModel);
    //                else    Debug.LogError("Не удалось создать Hider компонент для игрока!");
    //                break;
    //            }
    //        }
    //    }
    //}
    private Vector3 GetSpawnPosition()
    {
        Bounds bounds = spawnArea.GetComponent<BoxCollider>().bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, y, z);
    }

    // Метод для вызова при выборе роли через UI или другой механизм
    public void OnRoleSelected(PlayerRole selectedRole)
    {
    }
}