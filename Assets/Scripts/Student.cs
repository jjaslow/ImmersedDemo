using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Student
{

    public string name;
    public int id;
    public string gender;
    public string colorHex;
    public Color color;

    public int gradeAttending;
    public float currentGPA;

    public void Initialize()
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString("#" + colorHex, out newColor))
        {
            color = newColor;
        }
    }
}

[System.Serializable]
public class StudentRoot
{
    public List<Student> students = new List<Student>();
}