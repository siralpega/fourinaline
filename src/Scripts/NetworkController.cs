using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkController : MonoBehaviourPunCallbacks

{
    public GameObject playerPrefab;
    void Start()
    {
        DontDestroyOnLoad(this);
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected to MatchMaking! Rooms: " + PhotonNetwork.CountOfRooms);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
      //  Debug.Log("Updating list of rooms on lobby " + PhotonNetwork.CurrentLobby.Name);
        //Updates/displays multiplayerjoin scene
        NetworkToggle.toggle(roomList);
    }

    public void joinGame(int id)
    {
        Debug.Log("Joining game: 4line_game_" + id);
        PhotonNetwork.JoinRoom("4line_game_" + id);
    }

    public void addGame()
    {
        int id = PhotonNetwork.CountOfRooms;

        var roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;
        Debug.Log("Creating game: 4line_game_" + id);
        if (!PhotonNetwork.CreateRoom("4line_game_" + id, roomOptions, TypedLobby.Default))
            Debug.LogError("Failed to create room 4line_game_" + id);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.PlayerList.Length <= 1 && PhotonNetwork.InRoom)
            PhotonNetwork.LoadLevel("MultiplayerLoadScene");
        else if (PhotonNetwork.InRoom)
        {
            // PhotonNetwork.LoadLevel("GameScene");
            SceneManager.LoadScene("GameScene");
            CreatePlayer();
        }
    }

    public void CreatePlayer()
    {
        //If we exist, destory on the server.
        var existing = GameObject.FindGameObjectWithTag("Player");
        if (existing != null)
            PhotonNetwork.Destroy(existing);

        var player = PhotonNetwork.Instantiate("Player", Vector2.zero, Quaternion.identity); //in resources folder, spawns that object. photon tells all the players too.

        int playerId;
        Player[] players = PhotonNetwork.PlayerList;
        if (players[0] == PhotonNetwork.LocalPlayer)
            playerId = 1;
        else
            playerId = 2;

        //Assignments to local player
        player.GetComponent<PlayerController>().player = playerId;
        player.name = "Player " + playerId;
        player.tag = "Player"; //sets my own tag to player.
        Color32 c = GameObject.Find("ColorToggle").GetComponent<ColorToggle>().color;
        player.GetComponent<PlayerController>().color = c;

        //Give my player info to everyone else who is connected
        PhotonView view = player.GetComponent<PhotonView>();
        view.RPC("AssignPlayersRPC", RpcTarget.Others, new int[] { c.r, c.g, c.b }, playerId); 
    }
}