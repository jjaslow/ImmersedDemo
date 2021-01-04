using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseHand : MonoBehaviourPun
{
    [SerializeField] Renderer handRenderer;

    MySyncronizationScript syncScript;

    bool isHandRaised = false;

    public enum RaiseEventCodes
    {
        LowerHand = 0,
        RaiseHand = 1
    }

    void Start()
    {
        syncScript = GetComponent<MySyncronizationScript>();
        GetComponent<MySyncronizationScript>().isHandRaised = false;

        PhotonNetwork.NetworkingClient.EventReceived += OnLowerHandEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnLowerHandEvent;
    }

    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.One))
            RaiseStudentsHand();


        if(syncScript.isHandRaised)
        {
            handRenderer.enabled = true;
        }
        else
        {
            handRenderer.enabled = false;
        }
    }



    //raise my student's hand locally. will sync automatically.
    public void RaiseStudentsHand()
    {
        if (isHandRaised)
            return;

        isHandRaised = true;

        SendRaiseHandMessage();

        if (photonView.IsMine)
            syncScript.isHandRaised = true;
    }

    public void LowerStudentHand()
    {
        if (photonView.IsMine)
        {
            isHandRaised = false;
            syncScript.isHandRaised = false;
        }
    }





    //accept RPC to lower hand for my remote player
    private void OnLowerHandEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseEventCodes.LowerHand)
        {
            //get the data sent from the event
            object[] data = (object[])photonEvent.CustomData;

            int viewID = (int)data[0];

            if (viewID == photonView.ViewID)
            {
                LowerStudentHand();
            }
        }
    }

    
    void SendRaiseHandMessage()
    {
        object[] data = new object[]
        {
            photonView.ViewID,
            photonView.Owner.NickName
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.All,
            CachingOption = EventCaching.DoNotCache
        };

        SendOptions sendOptions = new SendOptions
        {
            Reliability = true
        };

        PhotonNetwork.RaiseEvent((byte)RaiseHand.RaiseEventCodes.RaiseHand, data, raiseEventOptions, sendOptions);
    }
}
