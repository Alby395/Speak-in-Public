using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManagerStandalone : ActivityManager
{
    private int ActivityDuration;

    protected override void Init()
    {
        PercentageOfDistractedPeople = Mathf.FloorToInt(peopleManagers.Length * PlayerPrefs.GetInt("PercentageOfDistractedPeople") / 100);
        for(int i = 0; i < PercentageOfDistractedPeople; i++)
        {
            peopleManagers[i].StartDistraction();
        }
        
        ActivityDuration = PlayerPrefs.GetInt("ActivityDuration", 5);
        StartCoroutine(Timer());
    }

    
    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(ActivityDuration * 60f);
        
        EventManager.TriggerEvent("StartCheering");
        StartCoroutine(ActivityTerminated());
    }
}
