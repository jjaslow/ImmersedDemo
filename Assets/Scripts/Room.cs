using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    Teacher teacher;
    List<Student> students = new List<Student>();
    public int maxClassSize;


}
