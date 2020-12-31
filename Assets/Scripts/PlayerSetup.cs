using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] CharacterController characterController;
    [SerializeField] OVRPlayerController oVRPlayerController;
    [SerializeField] GameObject cameraRig;

    private void Start()
    {
        if(!PhotonNetwork.LocalPlayer.IsLocal)
        {
            characterController.enabled = false;
            oVRPlayerController.enabled = false;
            cameraRig.SetActive(false);
        }
    }
}
