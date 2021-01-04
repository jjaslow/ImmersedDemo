using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    TeacherRoot teacherList;
    StudentRoot studentList;
    [SerializeField]
    List<Room> classRooms = new List<Room>();


    [Header("Identification")]
    int teacherNumber = -1;
    int studentNumber = -1;
    int classNumber = -1;

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
    [SerializeField] public TMP_Text studentNoticeText;

    private void Start()
    {
        mainPanel.SetActive(true);
        teacherPanel.SetActive(false);
        studentPanel.SetActive(false);

        //download JSON from API here
        StartCoroutine(DownloadJSON.DownloadJSONFiles());

        //process hardcoded JSON files and create lists of teachers and students
        TextAsset jsonTeacher = Resources.Load<TextAsset>("teachers");     
        teacherList = JsonUtility.FromJson<TeacherRoot>(jsonTeacher.text);

        TextAsset jsonStudent = Resources.Load<TextAsset>("students");
        studentList = JsonUtility.FromJson<StudentRoot>(jsonStudent.text);
        foreach (var s in studentList.students)
        {
            s.Initialize(); //set the student avatar color (from Hex value)
        }
    }




    public void SwitchScene()
    {
        SceneManager.LoadScene("ClassRoom");
    }


    public IEnumerator ClearStudentNoticeText()
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

        //load teachers into dropdown
        List<string> options = new List<string>();
        foreach (var teacher in teacherList.teachers)
        {
            options.Add(teacher.classCode);
        }
        teacherDropdown.ClearOptions();
        teacherDropdown.AddOptions(options);
    }

    public void TeacherGotoClass()
    {
        teacherNumber = teacherDropdown.value;
        Debug.Log(teacherList.teachers[teacherNumber].professor);
        debugText.text += teacherList.teachers[teacherNumber].professor + "\n";

        //new Room(teacherList.teachers[teacherNumber]);

        NetworkManager.Instance.CreateRoom(teacherList, teacherNumber);
    }



    public void StudentSelectClass()
    {
        mainPanel.SetActive(false);
        teacherPanel.SetActive(false);
        studentPanel.SetActive(true);

        //load students into dropdown
        List<string> options = new List<string>();
        foreach (Student student in studentList.students)
        {
            options.Add(student.name);
        }
        studentDropdown.ClearOptions();
        studentDropdown.AddOptions(options);

        //load teachers into dropdown
        List<string> options2 = new List<string>();
        foreach (var teacher in teacherList.teachers)
        {
            options2.Add(teacher.classCode);
        }
        studentClassesDropdown.ClearOptions();
        studentClassesDropdown.AddOptions(options2);
    }

    public void StudentGotoClass()
    {
        studentNumber = studentDropdown.value;
        classNumber = studentClassesDropdown.value;

        NetworkManager.Instance.JoinRoom(teacherList, studentList, studentNumber, classNumber);
    }


    #endregion




}
