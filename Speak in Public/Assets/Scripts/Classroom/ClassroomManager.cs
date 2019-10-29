using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.XR;
using UnityEditor;
using WebSocketSharp;
using UnityEngine.Events;

public class ClassroomManager : MonoBehaviour
{
    private int NumberOfPeople;
    private float PercentageOfDistractedPeople;
    private bool MicrophoneEnabled;

    public GameObject people;
    private List<Animator> animators;

    private delegate void Event();
    private event Event Consumer;

    private void Awake()
    {
        NumberOfPeople = PlayerPrefs.GetInt("NumberOfPeople");
        PercentageOfDistractedPeople = Mathf.FloorToInt(PlayerPrefs.GetInt("NumberOfPeople") * PlayerPrefs.GetInt("PercentageOfDistractedPeople") / 100);
        MicrophoneEnabled = (PlayerPrefs.GetInt("MicrophoneEnabled") == 0) ? false : true;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.EnableVR();

        System.Random rnd = new System.Random();
        animators = people.GetComponentsInChildren<Animator>().OrderBy(x => rnd.Next()).ToList();
        for (int i = 0; i < 8 - NumberOfPeople; i++)
        {
            Destroy(animators[i].gameObject);
        }

        animators = people.GetComponentsInChildren<Animator>().OrderBy(x => rnd.Next()).ToList();
        for (int i = 0; i < PercentageOfDistractedPeople; i++)
        {
            animators[i].SetBool("Reactive", true);
        }

        transform.GetComponent<MicrophoneManager>().enabled = MicrophoneEnabled;


        EventManager.StartListening("ReturnToMenu", GameManager.instance.GoToMenu);

        if (GameManager.instance.TWBenabled && WebSocketManager.instance.GetStatus() == WebSocketState.Open)
        {
            WebSocketManager.instance.AddHandlerMessage(MessageHandler);
        }       
    }

    private void Update()
    {
        Consumer?.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.GoToMenu();
        }
    }


    public void DetectSpeech()
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("SpeechDetected");
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance.TWBenabled)
        {
            WebSocketManager.instance.RemoveHandlerMessage(MessageHandler);
        }
    }


    private void MessageHandler(object sender, MessageEventArgs e)
    {
        Message msg = JsonUtility.FromJson<Message>(e.Data);
        print(msg);
        if (msg.type == "StartCheering")
        {
            Consumer += TerminateActivity;
        }
        if (msg.type == "ReturnToMenu")
        {
            Consumer += GoToMenu;
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
        if (GameManager.instance.TWBenabled)
        {
            StartCoroutine(ActivityTerminated());
        }
    }

    private IEnumerator ActivityTerminated()
    {
        yield return new WaitForSeconds(10f);
        GameManager.instance.GoToMenu();
    }

    private void GoToMenu()
    {
        EventManager.TriggerEvent("GoToMenu");
        Consumer -= GoToMenu;
    }

    
    [Serializable]
    private class Message
    {
        public string type;
        public string message;
    }
}
