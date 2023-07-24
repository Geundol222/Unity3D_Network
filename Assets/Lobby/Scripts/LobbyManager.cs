using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Menu, Lobby, Room }

    [SerializeField] StatePanel statePanel;
    [SerializeField] LoginPanel loginPanel;
    [SerializeField] MenuPanel menuPanel;
    [SerializeField] RoomPanel roomPanel;
    [SerializeField] LobbyPanel lobbyPanel;

    private void Start()
    {
        SetActivePanel(Panel.Login);
    }

    public override void OnConnectedToMaster()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SetActivePanel(Panel.Login);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Create Room Failed With Error({returnCode}) : {message}");
        statePanel.AddMessage($"Create Room Failed With Error({returnCode}) : {message}");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join Room Failed With Error({returnCode}) : {message}");
        statePanel.AddMessage($"Join Room Failed With Error({returnCode}) : {message}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        SetActivePanel(Panel.Menu);
        Debug.Log($"Join Random Room Failed With Error({returnCode}) : {message}");
        statePanel.AddMessage($"Join Random Room Failed With Error({returnCode}) : {message}");
    }

    public override void OnJoinedRoom()
    {
        SetActivePanel(Panel.Room);

        // TODO : 방에 들어갔을 때 작업
    }

    public override void OnJoinedLobby()
    {
        SetActivePanel(Panel.Lobby);

        // TODO : 로비에 들어갔을 때 작업
    }

    public override void OnLeftLobby()
    {
        SetActivePanel(Panel.Menu);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        lobbyPanel.UpdateRoomList(roomList);
    }

    private void SetActivePanel(Panel panel)
    {
        loginPanel.gameObject?.SetActive(panel == Panel.Login);
        menuPanel.gameObject?.SetActive(panel == Panel.Menu);
        roomPanel.gameObject?.SetActive(panel == Panel.Room);
        lobbyPanel.gameObject?.SetActive(panel == Panel.Lobby);
    }
}
