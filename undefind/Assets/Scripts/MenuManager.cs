using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    //[SerializeField] private Button startButton;
    //[SerializeField] private Button optionsButton;
    //[SerializeField] private Button quitButton;

    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    // Create Room Method
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(createInput.text, roomOptions);
        }
        else
        {
            Debug.LogError("Not connected to Photon servers.");
        }
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            Debug.LogError("Not connected to Photon servers.");
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room Successfully!");
        PhotonNetwork.LoadLevel(Scene.GameScene.ToString());
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to Photon Master Server.");
        
        PhotonNetwork.JoinLobby();
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //private void Start()
    //{
    //    startButton.onClick.AddListener(OnStartButtonClicked);
    //    optionsButton.onClick.AddListener(OnOptionsButtonClicked);
    //    quitButton.onClick.AddListener(OnQuitButtonClicked);
    //}

    //private void OnStartButtonClicked()
    //{
    //    Loader.Load(Scene.SampleScene);
    //}

    //private void OnOptionsButtonClicked()
    //{
    //}

    //private void OnQuitButtonClicked()
    //{
    //}
}
