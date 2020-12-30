using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Student : MonoBehaviour
{
    public enum Gender
    {
        Male = 0,
        Female = 1,
        Other = 3
    }

    string studentName;
    int studentIDNumber;
    Gender studentGender;
    Color avatarColor;

    int gradeAttending;
    float currentGPA;


    #region Getters
    public string GetStudentName()
    {
        return studentName;
    }
    public int GetStudentID()
    {
        return studentIDNumber;
    }
    public Gender GetStudentGender()
    {
        return studentGender;
    }
    public Color GetAvatarColor()
    {
        return avatarColor;
    }
    public int GetGradeAttending()
    {
        return gradeAttending;
    }
    public float GetStudentGPA()
    {
        return currentGPA;
    }
    #endregion






}
