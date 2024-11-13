using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviourPunCallbacks
{
    public GameObject RoomPrefab;
    public GameObject RoomListContent;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in RoomListContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            if (!roomInfo.RemovedFromList)
            {
                GameObject room = Instantiate(RoomPrefab, Vector3.zero, Quaternion.identity, RoomListContent.transform);
                room.GetComponent<RoomItem>().Name.text = roomInfo.Name;
                Debug.Log("Room added to list: " + roomInfo.Name);
            }
        }
    }
}
