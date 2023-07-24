using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] RectTransform playerContent;
    [SerializeField] PlayerEntry playerEntryPrefab;
    [SerializeField] Button startButton;

    public void UpdatePlayerList()
    {
        // Clear Player List
        for (int i = 0; i < playerContent.childCount; i++)
        {
            Destroy(playerContent.GetChild(i).gameObject);
        }

        // Update Player List
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            PlayerEntry entry = Instantiate(playerEntryPrefab, playerContent);
            entry.SetPlayerInfo(player);
        }
        
        if (PhotonNetwork.IsMasterClient)
            CheckPlayerReady();
        else
            startButton.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void CheckPlayerReady()
    {
        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 만약 플레이어의 레디 커스텀 프로퍼티가 true 이면
            if (player.GetReady())
                readyCount++;
        }

        startButton.gameObject.SetActive(readyCount == PhotonNetwork.PlayerList.Length);
    }
}
