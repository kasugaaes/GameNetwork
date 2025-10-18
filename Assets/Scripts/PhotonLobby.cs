using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using System.Collections;
using System.Collections.Generic;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    [Header("Pohton Connection")]
    public static PhotonLobby lobby; //itself
    public string sceneToLoad;

    [Header("Object References")]
    public GameObject createButton;
    public GameObject joinButton;
    public GameObject offlineButton;
    public GameObject cancelButton;

    public InputField joinID;
    public InputField createID;


    private void Awake() //before start
    {
        lobby = this;
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to server
        offlineButton.gameObject.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon Master server");
        //PhotonNetwork.AutomaticallySyncScene = true;
        createButton.SetActive(true); //this enables the start button allowing players to create a network  lobby
        joinButton.SetActive(true);//this enables the join button allowing players to join a network  lobby
        offlineButton.SetActive(false); //this will be disabled if we are connected to the server
    }

    //button script for connecting to a network game
    public void BtnCreateRoom()
    {
        Debug.Log("Play Button was clicked");
        Debug.Log("Creating a room...");
        CreateRoom();

        createButton.SetActive(false);
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
    }

    //button script for joining a network game
    public void BtnJoinRoom()
    {
        Debug.Log("Joining a room...");
        JoinRoomById();

        createButton.SetActive(false);
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
    }

    //button script for canceling connection to a network game
    public void OnCancelButtonClicked()
    {
        Debug.Log("Cancel Button Clicked");
        cancelButton.SetActive(false);
        PhotonNetwork.LeaveRoom();
        createButton.SetActive(true);
        joinButton.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random game but failed. There must be no open room");
        CreateRoom();
    }

    public void CreateRoom()
    {
        string roomName = createID.text;
        if (string.IsNullOrEmpty(roomName)) //creates a random room if no input data detected
        {
            roomName = "" + Random.Range(1000, 9999); // fallback if empty
        }

        RoomOptions roomOps = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };

        PhotonNetwork.CreateRoom(roomName, roomOps);
        Debug.Log($"Trying to create room: {roomName}");
    }
    public void JoinRoomById()
    {
        string roomToJoin = joinID.text.Trim();

        if (string.IsNullOrEmpty(roomToJoin)) //joins a  random room if no input data detected
        {
            Debug.Log("No room name entered → joining a random open room...");
            PhotonNetwork.JoinRandomRoom(); // will trigger OnJoinRandomFailed if none exists
        }
        else //this allows us to connect to the specified room
        {
            Debug.Log($"Trying to join specific room: {roomToJoin}");
            PhotonNetwork.JoinRoom(roomToJoin);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("We are now in a room");
        PhotonNetwork.LoadLevel(sceneToLoad);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must be a room with the same name");
        CreateRoom();
    }
}
