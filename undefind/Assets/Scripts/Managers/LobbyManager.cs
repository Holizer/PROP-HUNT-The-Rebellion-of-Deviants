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

    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            statusText.text = isReady ? "�� ������!" : "������� ��������� �������";
        }
    }

    public void OnReadyButtonPressed()
    {
        isReady = !isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new PhotonHashtable { { "isReady", isReady } });
        UpdateStatusText();
    }

    private void CheckAllPlayersReady()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return;

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

        // ���������� ��� ������������, ����� ����� �������� ����
        int playerIndex = 0;

        while (timeLeft > 0)
        {
            // ��������� ���� ���������� ������ �� ����� �������
            if (PhotonNetwork.CurrentRoom.PlayerCount > playerIndex)
            {
                AssignRoleToPlayer(playerIndex);
                playerIndex++;
            }

            // ��������� ����� �������
            UpdateCountdownText($"���� ������� ����� {timeLeft}...");
            yield return new WaitForSeconds(1);  // �������� 1 �������
            timeLeft--;
        }

        // ����� ������ ����������, ���������� ���������
        UpdateCountdownText("���� ����������!");
        yield return new WaitForSeconds(1);  // �������� ����� ������� ����

        // �������� ����
        LogPlayerRoles();

        // ��������� ����
        StartGame();
    }

    private void UpdateCountdownText(string text)
    {
        if (countdownText != null)
        {
            countdownText.text = text;
        }
    }
    private void AssignRoleToPlayer(int playerIndex)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        Player player = PhotonNetwork.CurrentRoom.Players.Values.ElementAt(playerIndex);

        string role = (playerIndex == 0) ? "Hunter" : "Hider";

        PhotonHashtable playerProperties = new PhotonHashtable { { "Role", role } };
        player.SetCustomProperties(playerProperties);

        Debug.Log($"���� ���������: ����� {player.NickName} � ����: {role}");
    }

    private void LogPlayerRoles()
    {
        Debug.Log("������ ������� � �� ����:");
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.CustomProperties.TryGetValue("Role", out object role))
            {
                Debug.Log($"����� {player.NickName} � ����: {role}");
            }
            else
            {
                Debug.Log($"����� {player.NickName} � ���� �� ���������.");
            }
        }
    }


    private void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(Scene.GameMap.ToString());
        }
    }
}
