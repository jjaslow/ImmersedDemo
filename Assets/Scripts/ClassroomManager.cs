using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ClassroomManager : MonoBehaviourPunCallbacks
{
    public static ClassroomManager Instance { get; private set; }

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


    string nickname;
    string roomName;

    [SerializeField] GameObject teacherPrefab;
    [SerializeField] Transform teacherSpawnPoint;
    [SerializeField] GameObject studentPrefab;
    [SerializeField] Transform studentSpawnPoint;

    public List<GameObject> peopleInClassroom = new List<GameObject>();

    private void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnRaiseHandEvent;

        if (NetworkManager.Instance.isTeacher)
        {
            nickname = NetworkManager.Instance.teacherList.teachers[NetworkManager.Instance.teacherNumber].professor;

            GameObject teacherInstance = PhotonNetwork.Instantiate(teacherPrefab.name, teacherSpawnPoint.position, teacherSpawnPoint.transform.rotation);

            Teacher teach = NetworkManager.Instance.teacherList.teachers[NetworkManager.Instance.teacherNumber];
            string teachProfessor = teach.professor;
            string teachSubject = teach.subject;
            string teachClassCode = teach.classCode;

            teacherInstance.GetComponentInChildren<PlayerSetup>().SetNameText($"{teachProfessor}\n{teachSubject}\n{teachClassCode}");
            teacherInstance.GetComponentInChildren<PlayerSetup>().SetColor(Color.white);
            teacherInstance.GetComponentInChildren<PlayerSetup>().SetIsTeacher(true);

            roomName = NetworkManager.Instance.teacherList.teachers[NetworkManager.Instance.teacherNumber].subject;
        }
        else
        {
            nickname = NetworkManager.Instance.studentList.students[NetworkManager.Instance.studentNumber].name;

            GameObject studentInstance = PhotonNetwork.Instantiate(studentPrefab.name, studentSpawnPoint.position, studentSpawnPoint.transform.rotation);

            Student stud = NetworkManager.Instance.studentList.students[NetworkManager.Instance.studentNumber];
            string studentName = stud.name;
            int studentID = stud.id;
            int studentGrade = stud.gradeAttending;

            studentInstance.GetComponentInChildren<PlayerSetup>().SetNameText($"{studentName}\n{studentID}\nGrade {studentGrade}");
            studentInstance.GetComponentInChildren<PlayerSetup>().SetColor(stud.color);
            studentInstance.GetComponentInChildren<PlayerSetup>().SetIsTeacher(false);


            roomName = NetworkManager.Instance.teacherList.teachers[NetworkManager.Instance.classNumber].subject;
        }

        PhotonNetwork.LocalPlayer.NickName = nickname;

    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnRaiseHandEvent;
    }




    private void Update()
    {
        if (peopleInClassroom.Count > 7)
            MakePlayerInactive();
    }


    public void MakePlayerInactive()
    {
        List<GameObject> possibleToHide = new List<GameObject>();
        foreach (GameObject go in peopleInClassroom)
        {
            if (!go.GetComponentInChildren<MySyncronizationScript>().isTeacher && go.activeSelf)
                possibleToHide.Add(go);
        }

        possibleToHide[0].gameObject.SetActive(false);
    }


    //if a student raises their hand, we also want to make them active (and hide someone else) if they are hidden
    void OnRaiseHandEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseHand.RaiseEventCodes.RaiseHand)
        {
            //get the data sent from the event
            object[] data = (object[])photonEvent.CustomData;

            int viewID = (int)data[0];
            ReactivatePlayer(viewID);
        }
    }

    public void ReactivatePlayer(int playerID)
    {
        int index;
        for (index = 0; index < peopleInClassroom.Count; index++)
        {
            int viewID = peopleInClassroom[index].GetComponentInChildren<PhotonView>().ViewID;
            if (viewID == playerID)
                break;
        }

        if(!peopleInClassroom[index].activeSelf)
        {
            MakePlayerInactive();
            peopleInClassroom[index].SetActive(true);
        }

    }
}
