using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchUI : MonoBehaviour
{
    public List<GameObject> canvas;
    int currentIndex = 0;

    [SerializeField]
    Text nameText;

    public void NextUI()
    {
        if (canvas.Count == 0)
        {
            return;
        }

        canvas[currentIndex].SetActive(false);
        currentIndex += 1; // Increment current index
        if (currentIndex >= canvas.Count)
        {
            LoadOfficeScene();
            return;
        }
        canvas[currentIndex].SetActive(true);
    }

    void LoadOfficeScene()
    {
        PlayerPrefs.SetString("playerName", nameText.text);
        SceneManager.LoadScene(1);
    }
}
