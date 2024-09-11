using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject hiderPrefab;
    public GameObject hunterPrefab;

    public Transform[] hiderSpawnPoints;
    public Transform[] hunterSpawnPoints;

    private List<Player> players = new List<Player>();

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CreatePlayer(PlayerRole.Hider, hiderSpawnPoints[0]);
        CreatePlayer(PlayerRole.Hunter, hunterSpawnPoints[0]);
    }

    private void CreatePlayer(PlayerRole role, Transform spawnpoint)
    {
        GameObject playerObject;

        if (role == PlayerRole.Hider)
        {
            playerObject = Instantiate(hiderPrefab, spawnpoint.position, spawnpoint.rotation);
            players.Add(playerObject.GetComponent<Hider>());
        }
        else if (role == PlayerRole.Hunter)
        {
            playerObject = Instantiate(hunterPrefab, spawnpoint.position, spawnpoint.rotation);
            Hunter hunter = playerObject.GetComponent<Hunter>();
            players.Add(hunter);
        }
    }
}
