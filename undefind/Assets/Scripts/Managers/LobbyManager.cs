using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using System.Collections;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using System.Linq;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerCountText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject readyButton;

    [SerializeField] private int countdownTime = 5; 
    private bool isReady = false; 
    private Coroutine countdownCoroutine;

    public bool isDebugMode = false;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        UpdatePlayerCount();
        AssignUniqueNickName();
        UpdateStatusText();
    }

    private void AssignUniqueNickName()
    {
        if (string.IsNullOrEmpty(PhotonNetwork.NickName))
        {
            PhotonNetwork.NickName = $"Player_{Random.Range(1000, 9999)}";
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
        CheckAllPlayersReady();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
        CheckAllPlayersReady();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey("isReady"))
        {
            CheckAllPlayersReady();
        }
    }
    
    private void UpdatePlayerCount()
    {
        if (playerCountText != null)
        {
            playerCountText.text = $"Players: {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
    }

    private void UpdateCountdownText(string text)
    {
        if (countdownText != null)
        {
            countdownText.text = text;
        }
    }
    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            statusText.text = isReady ? "Вы готовы!" : "Ожидаем остальных игроков";
        }
    }

    public void OnReadyButtonPressed()
    {
        isReady = !isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashtable { { "isReady", isReady } });
        UpdateStatusText();
    }

    public void OnLeaveRoomButtonPressed()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(Scene.RoomList.ToString());
    }

    private void CheckAllPlayersReady()
    {
        if (!isDebugMode && PhotonNetwork.CurrentRoom.PlayerCount < 2) return;

        bool allReady = true;

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (!player.CustomProperties.TryGetValue("isReady", out object isReadyObj) || !(bool)isReadyObj)
            {
                allReady = false;
                break;
            }
        }

        if (allReady)
        {
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(StartCountdown());
            }
        }
        else
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
                UpdateCountdownText("");
            }
        }
    }
    private IEnumerator StartCountdown()
    {
        int timeLeft = countdownTime;
        while (timeLeft > 0)
        {

            UpdateCountdownText($"Игра начнётся через {timeLeft}...");
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

        AssignRoleToPlayers();

        UpdateCountdownText("Игра начинается!");
        yield return new WaitForSeconds(1);

        LogPlayerRoles();

        StartGame();
    }
    private void AssignRoleToPlayers()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var players = PhotonNetwork.CurrentRoom.Players.Values.ToList();

        int hunterIndex = Random.Range(0, players.Count);

        for (int i = 0; i < players.Count; i++)
        {
            string role = (i == hunterIndex) ? "Hunter" : "Hider"; 

            PhotonHashtable playerProperties = new PhotonHashtable { { "Role", role } };
            players[i].SetCustomProperties(playerProperties);

            Debug.Log($"Роль назначена: Игрок {players[i].NickName} — Роль: {role}");
        }
    }
    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(Scene.GameMap2.ToString());
        }
    }


    private void LogPlayerRoles()
    {
        Debug.Log("Список игроков и их роли:");
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.CustomProperties.TryGetValue("Role", out object role))
            {
                Debug.Log($"Игрок {player.NickName} — Роль: {role}");
            }
            else
            {
                Debug.Log($"Игрок {player.NickName} — Роль не назначена.");
            }
        }
    }
}