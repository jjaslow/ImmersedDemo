using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections;

public class CampusManager : MonoBehaviourPunCallbacks
{
    public static CampusManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Lists")]
    public TeacherRoot teacherList;
    public StudentRoot studentList;
    [SerializeField]
    List<Room> classRooms = new List<Room>();

    [Header("Identification")]
    public bool isTeacher = false;
    public int teacherNumber = -1;
    public int studentNumber = -1;
    public int classNumber = -1;

    [Header("Dropdowns")]
    [SerializeField] TMP_Dropdown teacherDropdown;
    [SerializeField] TMP_Dropdown studentDropdown;
    [SerializeField] TMP_Dropdown studentClassesDropdown;

    [Header("Panels")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject teacherPanel;
    [SerializeField] GameObject studentPanel;

    [Header("Text Fields")]
    [SerializeField] TMP_Text debugText;
    [SerializeField] TMP_Text studentNoticeText;

    private void Start()
    {
        mainPanel.SetActive(true);
        teacherPanel.SetActive(false);
        studentPanel.SetActive(false);

        TextAsset jsonTeacher = Resources.Load<TextAsset>("teachers");     
        teacherList = JsonUtility.FromJson<TeacherRoot>(jsonTeacher.text);

        TextAsset jsonStudent = Resources.Load<TextAsset>("students");
        studentList = JsonUtility.FromJson<StudentRoot>(jsonStudent.text);
        foreach (var s in studentList.students)
        {
            s.Initialize();
        }
    }


    void SwitchScene()
    {
        SceneManager.LoadScene("ClassRoom");
    }


    IEnumerator ClearStudentNoticeText()
    {
        yield return new WaitForSeconds(3f);
        studentNoticeText.text = "";
    }



    #region UI Callbacks
    public void TeacherSelectClass()
    {
        mainPanel.SetActive(false);
        teacherPanel.SetActive(true);
        studentPanel.SetActive(false);

        List<string> options = new List<string>();
        foreach (var teacher in teacherList.teachers)
        {
            options.Add(teacher.classCode); // Or whatever you want for a label
        }
        teacherDropdown.ClearOptions();
        teacherDropdown.AddOptions(options);
    }

    public void TeacherGotoClass()
    {
        isTeacher = true;

        teacherNumber = teacherDropdown.value;
        Debug.Log(teacherList.teachers[teacherNumber].professor);
        debugText.text += teacherList.teachers[teacherNumber].professor + "\n";

        new Room(teacherList.teachers[teacherNumber]);

        NetworkManager.Instance.CreateRoom();
    }



    public void StudentSelectClass()
    {
        mainPanel.SetActive(false);
        teacherPanel.SetActive(false);
        studentPanel.SetActive(true);

        List<string> options = new List<string>();
        foreach (Student student in studentList.students)
        {
            options.Add(student.name); // Or whatever you want for a label
        }
        studentDropdown.ClearOptions();
        studentDropdown.AddOptions(options);


        List<string> options2 = new List<string>();
        foreach (var teacher in teacherList.teachers)
        {
            options2.Add(teacher.classCode); // Or whatever you want for a label
        }
        studentClassesDropdown.ClearOptions();
        studentClassesDropdown.AddOptions(options2);
    }

    public void StudentGotoClass()
    {
        studentNumber = studentDropdown.value;
        classNumber = studentClassesDropdown.value;

        NetworkManager.Instance.JoinRoom();
    }


    #endregion





    #region PUN Callbacks


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


    public override void OnJoinedRoom()
    {
        Debug.Log("JOINED room: " + PhotonNetwork.CurrentRoom.Name);
        debugText.text += "JOINED room: " + PhotonNetwork.CurrentRoom.Name + "\n";
        SwitchScene();
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join room FAILED: " + message);
        debugText.text += "Join room FAILED: " + message + "\n";
        studentNoticeText.text = "Class has not yet started. Please wait.";
        StartCoroutine(ClearStudentNoticeText());
    }


    #endregion


}
