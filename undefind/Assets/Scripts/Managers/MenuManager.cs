using Photon.Pun;
using UnityEngine;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public GameObject settingsPanel;
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        settingsPanel.SetActive(false);
    }

    public void OnMultiplayerButtonClicked()
    {
        Loader.Load(Scene.RoomList);
    }

    public void OnOptionsButtonClicked()
    {
        settingsPanel.SetActive(true);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
