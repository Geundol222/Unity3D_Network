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
            // ���� ����� ������ �� + ���� ������� �Ǿ��� �� + ���� ������ ��
            if (info.RemovedFromList || !info.IsVisible || !info.IsOpen)
            {
                if (roomDictionary.ContainsKey(info.Name)) 
                {
                    roomDictionary.Remove(info.Name);
                }

                continue;
            }

            // ���� �ڷᱸ���� ���� �� (������ �̸��� �־��� ���̸� �ֽ�ȭ)
            if (roomDictionary.ContainsKey(info.Name))
            {
                roomDictionary[info.Name] = info;
            }
            // ���� �ڷᱸ���� ���� �� (���� ���� ���̸�)
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
