using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] CharacterController characterController;
    [SerializeField] OVRPlayerController oVRPlayerController;
    [SerializeField] OVRSceneSampleController oVRSceneSampleController;
    [SerializeField] OVRDebugInfo oVRDebugInfo;
    [SerializeField] GameObject cameraRig;

    [SerializeField] GameObject avatar;
    [SerializeField] GameObject canvas;
    [SerializeField] TMP_Text nameTextCanvas;
    [SerializeField] Canvas questionsCanvas;

    string nameText;
    Color color;

    bool isTeacher;


    private void Start()
    {
        StartCoroutine(InitializePlayerDetails());

        DontDestroyOnLoad(gameObject);

        PhotonView pv = GetComponent<PhotonView>();

        //enable camera and movement for my player only.
        if(pv.IsMine)
        {
            Debug.Log("Player Setup isMine.");

            cameraRig.SetActive(true);
            characterController.enabled = true;
            oVRPlayerController.enabled = true;
            oVRSceneSampleController.enabled = true;
            oVRDebugInfo.enabled = true;

            if(isTeacher)
            {
                questionsCanvas.worldCamera = Camera.main;
            }
        }

        //add to master list of people in room
        ClassroomManager.Instance.peopleInClassroom.Add(gameObject);
    }


    //set player variables (name card, avatar color, isTeacher) on local player
    //will be synced across the network after
    public void SetNameText(string info)
    {
        nameText = info;
    }

    public void SetColor(Color c)
    {
        color = c;
    }

    public void SetIsTeacher(bool value)
    {
        isTeacher = value;
    }


    IEnumerator InitializePlayerDetails()
    {
        //quick way to avoid avatars showing up before scene changes to classroom
        //this is an issue only for remote players who need to join the room
        //before switching scene. We Instantiate upon switching scenes so currently
        //we need to join first, scene change second, instantiate third.
        yield return new WaitForSeconds(1);

        avatar.SetActive(true);
        canvas.SetActive(true);

        nameTextCanvas.text = nameText;

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            r.material.color = color;

        GetComponent<MySyncronizationScript>().isTeacher = isTeacher;
    }
}
