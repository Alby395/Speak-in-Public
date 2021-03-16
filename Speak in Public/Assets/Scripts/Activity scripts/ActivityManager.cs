using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.XR;
using UnityEditor;
using WebSocketSharp;
using UnityEngine.Events;
using TMPro;

public abstract class ActivityManager : MonoBehaviour
{
    private int NumberOfPeople;
    private float PercentageOfDistractedPeople;
    private bool MicrophoneEnabled;

    public GameObject TextTopic;
    public GameObject people;

    public static ActivityManager instance
    {
        get;
        private set;
    }

    public virtual void Awake()
    {
        if(GameManager.instance.TWBenabled)
        {
            if(this.GetType() == typeof(ActivityManagerStandalone))
            {
                Destroy(this);
            }
        }
        else
        {
            if(this.GetType() == typeof(ActivityManagerTWB))
            {
                Destroy(this);
            }
        }


        MicrophoneEnabled = (PlayerPrefs.GetInt("MicrophoneEnabled") == 0) ? false : true;
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        
        print("start");
        System.Random rnd = new System.Random();
        
        PersonManager[] peopleManagers = people.GetComponentsInChildren<PersonManager>().OrderBy(x => rnd.Next()).ToArray();

        NumberOfPeople = Math.Min(peopleManagers.Length, PlayerPrefs.GetInt("NumberOfPeople"));

        PercentageOfDistractedPeople = Mathf.FloorToInt(NumberOfPeople * PlayerPrefs.GetInt("PercentageOfDistractedPeople") / 100);

        for (int i = 0; i < peopleManagers.Length - NumberOfPeople; i++)
        {
            Destroy(peopleManagers[i].gameObject);
        }

        peopleManagers = people.GetComponentsInChildren<PersonManager>().OrderBy(x => rnd.Next()).ToArray();
        for (int i = 0; i < PercentageOfDistractedPeople; i++)
        {
            peopleManagers[i].SetDistract();
        }

        transform.GetComponent<MicrophoneManager>().enabled = MicrophoneEnabled;

        Init();
    }

    protected abstract void Init();

    protected IEnumerator ActivityTerminated()
    {
        yield return new WaitForSeconds(10f);
        GameManager.instance.GoToMenu();
    }

}


