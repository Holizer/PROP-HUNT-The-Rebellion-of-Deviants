using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField createInput;
    [SerializeField] private GameObject RoomItemPrefab;
    [SerializeField] private Transform RoomListContent;

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            Debug.LogError("Not connected to Photon servers.");
        }
    }
    
    public void OnGoBackButtonClicked()
    {
        Loader.Load(Scene.Menu);
    }

    public void OnRefreshButtonPressed()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            roomOptions.IsVisible = true;
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        else
        {
            Debug.LogError("Not connected to Photon servers or not in a lobby.");
        }
    }
    
    public static void JoinRoomInList(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(Scene.Lobby.ToString());
    }
}
