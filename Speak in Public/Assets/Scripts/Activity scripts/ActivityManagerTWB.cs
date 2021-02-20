using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class ActivityManagerTWB : ActivityManager
{
    private delegate void Event();
    private event Event Consumer;

    private GameObject Topic;
    private string newTopic;

    protected override void Init()
    {
        EventManager.StartListening("ReturnToMenu", GameManager.instance.GoToMenu);

        if (WebSocketManager.instance.GetStatus() == WebSocketState.Open)
        {
            WebSocketManager.instance.AddHandlerMessage(MessageHandler);
        }
    }

    protected override void Update()
    {
        Consumer?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.GoToMenu();
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
        if (msg.type == "Stop")
        {
            Consumer += TerminateActivity;
        }
        if (msg.type == "Command")
        {
            newTopic = msg.message;
            Consumer += SetTopic;
        }
        else
        {
            Debug.Log("Unknown type of message");
        }
    }

    private void TerminateActivity()
    {
        EventManager.TriggerEvent("StartCheering");
        Consumer -= TerminateActivity;
        StartCoroutine(ActivityTerminated());
    }

    private void GoToMenu()
    {
        EventManager.TriggerEvent("GoToMenu");
        Consumer -= GoToMenu;
    }

    private void SetTopic()
    {
        PlayerPrefs.SetString("Topic", newTopic);
        EventManager.TriggerEvent("UpdateTopic");
        Consumer -= SetTopic;
    }


    [Serializable]
    private class Message
    {
        public string type;
        public string message;
    }
}
