using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LowerHand : MonoBehaviourPun
{
    [SerializeField] TMP_Text questionsTextCanvas;
    Queue<int> questioneerList = new Queue<int>();

    private void Start()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnRaiseHandEvent;
    }

    void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnRaiseHandEvent;
    }

    //teacher answers the next question with A button press
    //TODO:: temp switch to button B as button A is read by both players in
    //my shared testing, so I need to break them apart.
    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two))
            AnswerQuestion();
    }

    private void AnswerQuestion()
    {
        //in case we press A without any questions
        if (questioneerList.Count == 0)
            return;

        int viewID = questioneerList.Dequeue();

        PhotonView otherView = null;
        foreach (GameObject go in ClassroomManager.Instance.peopleInClassroom)
        {
            otherView = go.transform.GetComponentInChildren<PhotonView>();
            if (otherView.ViewID == viewID)
                break;
        }   

        Debug.Log($"answered {otherView.Owner.NickName}");
        SendLowerHandMessage(otherView.ViewID);
        RemoveNameFromQuestionList();
    }

    //can also answer by colliding with the student
    private void OnTriggerEnter(Collider other)
    {
        PhotonView otherView = other.transform.GetComponentInChildren<PhotonView>();

        if (otherView == null)
            return;

        Debug.Log($"collided with {otherView.Owner.NickName}");
        SendLowerHandMessage(otherView.ViewID);
        RemoveNameFromQuestionList();
    }

    //so player will lower hand across the network
    void SendLowerHandMessage(int colliderViewID)
    {
        object[] data = new object[]
        {
            colliderViewID
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

        PhotonNetwork.RaiseEvent((byte)RaiseHand.RaiseEventCodes.LowerHand, data, raiseEventOptions, sendOptions);
    }


    //add question to teacher list so they know they have a question
    //this method should be moved elsewhere.
    void OnRaiseHandEvent(EventData photonEvent)
    {
        if (photonEvent.Code == (byte)RaiseHand.RaiseEventCodes.RaiseHand)
        {
            //get the data sent from the event
            object[] data = (object[])photonEvent.CustomData;

            int viewID = (int)data[0];
            string name = (string)data[1];

            questionsTextCanvas.text += name + "\n";
            questioneerList.Enqueue(viewID);
        }
    }


    void RemoveNameFromQuestionList()
    {
        var lines = questionsTextCanvas.text.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
        questionsTextCanvas.text = string.Join(System.Environment.NewLine, lines.Skip(1));
    }
}
