using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            // Topic
            case "Command":
                EventManager.TriggerEvent("UpdateTopic", msg.message);
                break;

            // Distraction - People
            case "Attention":
                EventManager.TriggerEvent("Reset");
                break;

            case "Single":
                EventManager.TriggerEvent("Reset");
                int rng = UnityEngine.Random.Range(0, peopleManagers.Length);
                peopleManagers[rng].StartDistraction();
                break;

            case "Partial":
                EventManager.TriggerEvent("Reset");
                System.Random rnd = new System.Random();
                peopleManagers = peopleManagers.OrderBy(x => rnd.Next()).ToArray();

                for(int i = 0; i < peopleManagers.Length/2; i++)
                {
                    peopleManagers[i].StartDistraction();
                }
                break;

            case "Chaos":
                EventManager.TriggerEvent("Reset");
                for(int i = 0; i < peopleManagers.Length; i++)
                {
                    peopleManagers[i].StartDistraction();
                }
                break;

            // Distraction - Audio
            case "Audio":
                EventManager.TriggerEvent("PlayDistraction", msg.message);
                break;
            
            // Distraction - Light
            case "Darkness":
                EventManager.TriggerEvent("Lights");
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
