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
        GameObject.Find("ActivitySettings").GetComponent<Settings>().NumberOfPeople = 4;
        GameObject.Find("ActivitySettings").GetComponent<Settings>().PercentageOfDistractedPeople = 1;
        GameObject.Find("ActivitySettings").GetComponent<Settings>().MicrophoneEnabled = true;
    }

    public void RegularSettings()
    {
        GameObject.Find("ActivitySettings").GetComponent<Settings>().NumberOfPeople = 6;
        GameObject.Find("ActivitySettings").GetComponent<Settings>().PercentageOfDistractedPeople = 1;
        GameObject.Find("ActivitySettings").GetComponent<Settings>().MicrophoneEnabled = true;
    }

    public void DifficultSettings()
    {
        GameObject.Find("ActivitySettings").GetComponent<Settings>().NumberOfPeople = 8;
        GameObject.Find("ActivitySettings").GetComponent<Settings>().PercentageOfDistractedPeople = 1;
        GameObject.Find("ActivitySettings").GetComponent<Settings>().MicrophoneEnabled = true;
    }
}
