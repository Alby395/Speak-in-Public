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
    
    private bool MicrophoneEnabled;

    public GameObject TextTopic;
    public GameObject people;
    
    protected PersonManager[] peopleManagers;

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
            GetComponent<MicrophoneManager>().enabled = (PlayerPrefs.GetInt("MicrophoneEnabled") == 0) ? false : true;
        }
    }

    protected IEnumerator ActivityTerminated()
    {
        yield return new WaitForSeconds(12f);
        GameManager.instance.GoToMenu();
    }
}


