using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    [SerializeField] RectTransform roomContent;
    [SerializeField] RoomEntry roomEntryPrefab;

    Dictionary<string, RoomInfo> roomDictionary;

    private void Awake()
    {
        roomDictionary = new Dictionary<string, RoomInfo>();
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        // Clear Room List
        for (int i = 0; i < roomContent.childCount; i++)
        {
            Destroy(roomContent.GetChild(i).gameObject);
        }

        // Update Room Info
        foreach (RoomInfo info in roomList)
        {
            // 방이 사라질 예정일 때 + 방이 비공개가 되었을 때 + 방이 닫혔을 때
            if (info.RemovedFromList || !info.IsVisible || !info.IsOpen)
            {
                if (roomDictionary.ContainsKey(info.Name)) 
                {
                    roomDictionary.Remove(info.Name);
                }

                continue;
            }

            // 방이 자료구조에 있을 때 (무조건 이름이 있었던 방이면 최신화)
            if (roomDictionary.ContainsKey(info.Name))
            {
                roomDictionary[info.Name] = info;
            }
            // 방이 자료구조에 없을 때 (지금 생긴 방이면)
            else
            {
                roomDictionary.Add(info.Name, info);
            }
        }

        // Create Room List
        foreach (RoomInfo info in roomDictionary.Values)
        {
            RoomEntry entry = Instantiate(roomEntryPrefab, roomContent);
            entry.SetRoomInfo(info);
        }
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }
}
