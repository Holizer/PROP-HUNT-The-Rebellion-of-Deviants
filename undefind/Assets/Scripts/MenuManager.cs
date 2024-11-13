using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        multiplayerButton.onClick.AddListener(OnMultiplayerButtonClicked);
        optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    public void OnMultiplayerButtonClicked()
    {
        Loader.Load(Scene.RoomList);
    }

    public void OnOptionsButtonClicked()
    {
    }

    public void OnQuitButtonClicked()
    {

    }
}
