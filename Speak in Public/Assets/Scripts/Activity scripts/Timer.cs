using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TMP_Text _timerText;
    
    private float _maxTime;

    private bool _stop;

    // Start is called before the first frame update
    void Start()
    {
        _timerText = GetComponent<TMP_Text>();

        _timerText.text = PlayerPrefs.GetInt("ActivityDuration") + ":00";

        if(GameManager.instance.TWBenabled)
        {
            EventManager.StartListening("StartRecording", StartTimer);
        }
        else
        {
            StartCoroutine(CountdownTimer());
        }

        EventManager.StartListening("StartCheering", StopTimer);  
    }

    private void StartTimer()
    {
        int setTime = PlayerPrefs.GetInt("ActivityDuration");

        if(setTime == 0)
            StartCoroutine(NormalTimer());
        else
            StartCoroutine(CountdownTimer());
    }

    private IEnumerator NormalTimer()
    {
        double currentTime = 0;
        
        while(!_stop)
        {
            yield return new WaitForSeconds(1f);
            currentTime += 1;

            _timerText.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss");
        }
    }

    private IEnumerator CountdownTimer()
    {
        double currentTime = PlayerPrefs.GetInt("ActivityDuration");
        print("Time (hh:mm) " + TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss"));

        currentTime *= 60;
        while(!_stop)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1;

            _timerText.text = TimeSpan.FromSeconds(currentTime).ToString(@"mm\:ss");
        }
    }

    private void StopTimer()
    {
        _stop = true;
    }
}
