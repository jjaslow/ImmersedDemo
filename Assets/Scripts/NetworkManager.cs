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

    public string roomName;
    public bool isTeacher = false;

    public TeacherRoot teacherList;
    public int teacherNumber;
    public StudentRoot studentList;
    public int studentNumber;
    public int classNumber;


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

    //teachers create classrooms
    public void CreateRoom(TeacherRoot tl, int tn)
    {
        teacherList = tl;
        teacherNumber = tn;

        Debug.Log("creating room");
        debugText.text += "creating room\n";

        roomName = teacherList.teachers[teacherNumber].subject;

        isTeacher = true;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 50;

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    //students join classrooms
    public void JoinRoom(TeacherRoot tl, StudentRoot sl, int sn, int cn)
    {
        teacherList = tl;
        studentList = sl;
        studentNumber = sn;
        classNumber = cn;

        Debug.Log("joining room: " + teacherList.teachers[classNumber].subject);
        debugText.text += "joining room: " + teacherList.teachers[classNumber].subject + "\n";

        roomName = teacherList.teachers[classNumber].subject;

        PhotonNetwork.JoinRoom(roomName);
    }



    #region PUN Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("CONNECTED to server: " + PhotonNetwork.CloudRegion);
        debugText.text += $"CONNECTED to server {PhotonNetwork.CloudRegion}\n";
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("CREATED room: " + PhotonNetwork.CurrentRoom.Name);
        debugText.text += "CREATED room: " + PhotonNetwork.CurrentRoom.Name + "\n";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create room FAILED: " + message);
        debugText.text += "Create room FAILED: " + message + "\n";
    }

    //both techers and students call this when they join the room.
    public override void OnJoinedRoom()
    {
        Debug.Log("JOINED room: " + PhotonNetwork.CurrentRoom.Name);
        debugText.text += "JOINED room: " + PhotonNetwork.CurrentRoom.Name + "\n";
        CampusManager.Instance.SwitchScene();
    }

    //student tried to join a room that doesnt exists (ie class hasnt been created / started yet)
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room FAILED: " + message);
        debugText.text += "Join room FAILED: " + message + "\n";
        CampusManager.Instance.studentNoticeText.text = "Class has not yet started. Please wait.";
        StartCoroutine(CampusManager.Instance.ClearStudentNoticeText());
    }

    #endregion
}
