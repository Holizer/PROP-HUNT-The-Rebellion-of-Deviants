using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
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
        Application.Quit();
    }
}
