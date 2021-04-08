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

    private Queue<Message> _messageQueue;


    protected override void Init()
    {
        EventManager.StartListening("Completed", EndSession);

        if (WebSocketManager.instance.GetStatus() == WebSocketState.Open)
        {
            WebSocketManager.instance.AddHandlerMessage(MessageHandler);
        }

        _messageQueue = new Queue<Message>();
        StartCoroutine(ManageMessage());  
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
        
        _messageQueue.Enqueue(msg);      
    }

    private IEnumerator ManageMessage()
    {
        do
        {
            while(_messageQueue.Count > 0)
            {
                Message msg = _messageQueue.Dequeue();

                switch (msg.type) 
                {
                    case "Start":
                        EventManager.TriggerEvent("StartRecording");
                        WebSocketManager.instance.SetAudioId(Int32.Parse(msg.id));
                        break;

                    case "Stop":
                        EventManager.TriggerEvent("StartCheering");
                        EventManager.TriggerEvent("StopRecording");
                        break;
                        
                    case "Command":
                        ParseCommand(msg);
                        break;            

                    default:
                        Debug.Log("Unknown type of message");
                        break;
                }
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

        }while(true);
    }
    
    private void ParseCommand(Message msg)
    {
        switch(msg.commandType)
        {
            case "distraction":
                switch(msg.message)
                {
                    case "none":                            // Tutti attenti
                        EventManager.TriggerEvent("Reset");
                        break;

                    case "one":                             // Uno distratto
                        EventManager.TriggerEvent("Reset");
                        int rng = UnityEngine.Random.Range(0, peopleManagers.Length);
                        peopleManagers[rng].DistractTWB();
                        break;

                    case "half":                            // Metà distratti
                        EventManager.TriggerEvent("Reset");
                        System.Random rnd = new System.Random();
                        peopleManagers = peopleManagers.OrderBy(x => rnd.Next()).ToArray();

                        for(int i = 0; i < peopleManagers.Length/2; i++)
                        {
                            peopleManagers[i].DistractTWB();
                        }
                        break;

                    case "all":                             // Tutti distratti
                        EventManager.TriggerEvent("Reset");
                        for(int i = 0; i < peopleManagers.Length; i++)
                        {
                            peopleManagers[i].DistractTWB();
                        }
                        break;
                }
                break;

            case "topic":
                EventManager.TriggerEvent("UpdateTopic", msg.message);
                break;   

            case "light_distraction":
                EventManager.TriggerEvent("Lights");
                break;

            case "audio_distraction":
                EventManager.TriggerEvent("PlayDistraction", msg.message);
                break;

            default:
                print("No command");
                break;
        }
    }

    private void EndSession()
    {
        StartCoroutine(ActivityTerminated());
        StopCoroutine("ManageMessage");
    }

    [Serializable]
    private class Message
    {
        public string type;
        public string commandType;
        public string message;
        public string id;
    }
}
