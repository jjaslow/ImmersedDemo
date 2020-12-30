using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public Teacher teacher;
    public List<Student> students = new List<Student>();
    public int maxClassSize;
    public int maxVisibleStudents = 7;

    public Room(Teacher t)
    {
        teacher = t;
    }

    public void AddStudent(Student newStudent)
    {
        students.Add(newStudent);
    }
}
