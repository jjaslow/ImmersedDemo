using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Teacher
{
    public string professor;
    public string subject;
    public string classCode;
}

[System.Serializable]
public class TeacherRoot
{
    public List<Teacher> teachers = new List<Teacher>();
}
