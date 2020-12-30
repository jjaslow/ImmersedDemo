using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampusManager : MonoBehaviour
{
    public static CampusManager Instance { get; private set; }

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


    [SerializeField]
    TeacherRoot teacherList;
    [SerializeField]
    StudentRoot studentList;
    [SerializeField]
    List<Room> classRooms = new List<Room>();

    [SerializeField] Dropdown teacherDropdown;
    [SerializeField] Dropdown studentDropdown;

    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject teacherPanel;


    private void Start()
    {
        mainPanel.SetActive(true);
        teacherPanel.SetActive(false);

        TextAsset jsonTeacher = Resources.Load<TextAsset>("teachers");     
        teacherList = JsonUtility.FromJson<TeacherRoot>(jsonTeacher.text);

        TextAsset jsonStudent = Resources.Load<TextAsset>("students");
        studentList = JsonUtility.FromJson<StudentRoot>(jsonStudent.text);
        foreach (var s in studentList.students)
        {
            s.Initialize();
        }
    }





    #region UI Callbacks
    public void TeacherSelectClass()
    {
        mainPanel.SetActive(false);
        teacherPanel.SetActive(true);

        List<string> options = new List<string>();
        foreach (Teacher teacher in teacherList.teachers)
        {
            options.Add(teacher.classCode); // Or whatever you want for a label
        }
        teacherDropdown.ClearOptions();
        teacherDropdown.AddOptions(options);
    }

    public void TeacherGotoClass()
    {
        int teacherNumber = teacherDropdown.value;
        Debug.Log(teacherList.teachers[teacherNumber].professor);

        Room newRoom = new Room(teacherList.teachers[teacherNumber]);
        classRooms.Add(newRoom);

        //SceneManager.LoadScene("ClassRoom");
    }

    public void StudentSelectClass()
    {

    }

    public void StudentGotoClass()
    {

    }


    #endregion







}
