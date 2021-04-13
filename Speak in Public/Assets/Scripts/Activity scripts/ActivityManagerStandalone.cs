using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivityManagerStandalone : ActivityManager
{
    private int NumberOfPeople;
    private float PercentageOfDistractedPeople;
    
    private int ActivityDuration;

    private void Start()
    {
        System.Random rnd = new System.Random();
        
        peopleManagers = people.GetComponentsInChildren<PersonManager>().OrderBy(x => rnd.Next()).ToArray();

        NumberOfPeople = Math.Min(peopleManagers.Length, PlayerPrefs.GetInt("NumberOfPeople"));

        for (int i = 0; i < peopleManagers.Length - NumberOfPeople; i++)
        {
            Destroy(peopleManagers[i].gameObject);
        }

        peopleManagers = peopleManagers.OrderBy(x => rnd.Next()).ToArray();

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
