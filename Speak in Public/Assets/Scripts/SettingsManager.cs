using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EasySettings()
    {
        GameObject.Find("Settings").GetComponent<Settings>().ClassroomDifficulty = 0;
        GameObject.Find("Settings").GetComponent<Settings>().NumberOfPeople = 4;
        GameObject.Find("Settings").GetComponent<Settings>().MicrophoneEnabled = true;
    }

    public void RegularSettings()
    {
        GameObject.Find("Settings").GetComponent<Settings>().ClassroomDifficulty = 1;
        GameObject.Find("Settings").GetComponent<Settings>().NumberOfPeople = 6;
        GameObject.Find("Settings").GetComponent<Settings>().MicrophoneEnabled = true;
    }

    public void DifficultSettings()
    {
        GameObject.Find("Settings").GetComponent<Settings>().ClassroomDifficulty = 1;
        GameObject.Find("Settings").GetComponent<Settings>().NumberOfPeople = 8;
        GameObject.Find("Settings").GetComponent<Settings>().MicrophoneEnabled = false;
    }
}
