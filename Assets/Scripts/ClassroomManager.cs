using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassroomManager : MonoBehaviour
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


    private void Start()
    {
        if (CampusManager.Instance.isTeacher)
        {
            nickname = CampusManager.Instance.teacherList.teachers[CampusManager.Instance.teacherNumber].professor;

            GameObject teacherInstance = PhotonNetwork.Instantiate(teacherPrefab.name, teacherSpawnPoint.position, teacherSpawnPoint.transform.rotation);

            Teacher teach = CampusManager.Instance.teacherList.teachers[CampusManager.Instance.teacherNumber];
            string teachProfessor = teach.professor;
            string teachSubject = teach.subject;
            string teachClassCode = teach.classCode;

            teacherInstance.GetComponent<PlayerSetup>().SetNameText($"{teachProfessor}\n{teachSubject}\n{teachClassCode}");
            //teacherInstance.GetComponentInChildren<TMP_Text>().text = $"{teachProfessor}\n{teachSubject}\n{teachClassCode}";

            teacherInstance.GetComponent<PlayerSetup>().SetColor(Color.white);

            roomName = CampusManager.Instance.teacherList.teachers[CampusManager.Instance.teacherNumber].subject;
        }
        else
        {
            nickname = CampusManager.Instance.studentList.students[CampusManager.Instance.studentNumber].name;

            GameObject studentInstance = PhotonNetwork.Instantiate(studentPrefab.name, studentSpawnPoint.position, studentSpawnPoint.transform.rotation);

            Student stud = CampusManager.Instance.studentList.students[CampusManager.Instance.studentNumber];
            string studentName = stud.name;
            int studentID = stud.id;
            int studentGrade = stud.gradeAttending;

            string nameText = $"{studentName}\n{studentID}\nGrade {studentGrade}";
            studentInstance.GetComponent<PlayerSetup>().SetNameText(nameText);
            //studentInstance.GetComponentInChildren<TMP_Text>().text = nameText;


            Color myColor = stud.color;
            studentInstance.GetComponent<PlayerSetup>().SetColor(myColor);
 

            PlayerSetup ps = studentInstance.GetComponent<PlayerSetup>();
            ps.SetNameText(nameText);
            ps.SetColor(myColor);

            roomName = CampusManager.Instance.teacherList.teachers[CampusManager.Instance.classNumber].subject;
        }

        PhotonNetwork.LocalPlayer.NickName = nickname;

    }

}
