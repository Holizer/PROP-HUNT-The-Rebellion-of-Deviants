using TMPro;
using UnityEngine;

public class RoomItem : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public void JoinRoom()
    {
        RoomListManager.JoinRoomInList(Name.text);
    }
}