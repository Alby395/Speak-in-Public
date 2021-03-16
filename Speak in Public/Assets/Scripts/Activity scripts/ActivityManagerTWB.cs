using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ActivityManagerTWB : ActivityManager
{
    private delegate void Event();

    private GameObject Topic;
    private string newTopic;

    protected override void Init()
    {
        EventManager.StartListening("Completed", EndSession);

        if (WebSocketManager.instance.GetStatus() == WebSocketState.Open)
        {
            WebSocketManager.instance.AddHandlerMessage(MessageHandler);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance.TWBenabled)
        {
            WebSocketManager.instance.RemoveHandlerMessage(MessageHandler);
        }
    }

    // Sarebbe da togliere gli if
    private void MessageHandler(object sender, MessageEventArgs e)
    {
        Message msg = JsonUtility.FromJson<Message>(e.Data);

        print(msg);
        
        //TODO Aggiungere gli altri messaggi
        switch (msg.type) 
        {
            case "Stop":
                EventManager.TriggerEvent("StartCheering");
                EventManager.TriggerEvent("StopRecording");
                break;
            case "Command":
                EventManager.TriggerEvent("UpdateTopic", msg.message);
                break;
            default:
                Debug.Log("Unknown type of message");
                break;
        }
    }


    private void EndSession()
    {
        StartCoroutine(ActivityTerminated());
    }

    [Serializable]
    private class Message
    {
        public string type;
        public string message;
    }
}
