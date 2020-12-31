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

    string nameText;
    Color color;

    public bool isVisible = false;

    private void Start()
    {
        StartCoroutine(MakeVisible());

        DontDestroyOnLoad(gameObject);

        PhotonView pv = GetComponent<PhotonView>();

        if(pv.IsMine)
        {
            Debug.Log("Player Setup isMine.");

            cameraRig.SetActive(true);
            characterController.enabled = true;
            oVRPlayerController.enabled = true;
            oVRSceneSampleController.enabled = true;
            oVRDebugInfo.enabled = true;
        }
    }

    public void SetNameText(string info)
    {
        nameText = info;
    }

    public void SetColor(Color c)
    {
        color = c;
    }

    IEnumerator MakeVisible()
    {
        yield return new WaitForSeconds(1.5f);
        avatar.SetActive(true);
        canvas.SetActive(true);

        GetComponentInChildren<TMP_Text>().text = nameText;

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rends)
            r.material.color = color;

        isVisible = true;
    }
}
