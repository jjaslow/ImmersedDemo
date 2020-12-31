using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    string roomName;
    [SerializeField] TMP_Text debugText;

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        Debug.Log("connecting to server");
        debugText.text += "connecting to server\n";

        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        Debug.Log("creating room");
        debugText.text += "creating room\n";

        roomName = CampusManager.Instance.teacherList.teachers[CampusManager.Instance.teacherNumber].subject;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 50;


        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void JoinRoom()
    {
        Debug.Log("joining room: " + CampusManager.Instance.teacherList.teachers[CampusManager.Instance.classNumber].subject);
        debugText.text += "joining room: " + CampusManager.Instance.teacherList.teachers[CampusManager.Instance.classNumber].subject + "\n";

        roomName = CampusManager.Instance.teacherList.teachers[CampusManager.Instance.classNumber].subject;

        PhotonNetwork.JoinRoom(roomName);
    }



    #region PUN Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED to server: " + PhotonNetwork.CloudRegion);
        debugText.text += $"CONNECTED to server {PhotonNetwork.CloudRegion}\n";
    }



    #endregion
}
