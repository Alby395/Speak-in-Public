using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManagerStandalone : ActivityManager
{
    private int ActivityDuration;

    protected override void Init()
    {
        ActivityDuration = PlayerPrefs.GetInt("ActivityDuration");
        StartCoroutine(Timer());
    }

    
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(ActivityDuration * 60f);
        
        EventManager.TriggerEvent("StartCheering");
        StartCoroutine(ActivityTerminated());
    }
}
