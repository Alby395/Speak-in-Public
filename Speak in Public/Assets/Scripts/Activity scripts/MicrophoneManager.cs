using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Audio;

public class MicrophoneManager : MonoBehaviour
{
    public static float MicLoudness;
    private string _device;
    private DateTime _startTime;

    AudioClip _clipRecord;
    int _sampleWindow = 128;
    
    bool _isInitialized;

    void OnEnable()
    {
        if (_device == null)
            _device = Microphone.devices[0];

        _isInitialized = true;
        
        if(GameManager.instance.TWBenabled)
        {
            EventManager.StartListening("StartRecording", StartMicrophone);
            EventManager.StartListening("StopRecording", StopMicrophone);
        }
        else
        {
            StartCoroutine(MicrophoneCheck());
            StartMicrophone();
        }
    }

    private IEnumerator AskForMicPermission()
    {
        //while (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }   
    }

    private void StartMicrophone()
    {
        _clipRecord = Microphone.Start(_device, false, 600, 44100);
        _startTime = DateTime.Now;
    }   

    void StopMicrophone()
    {
        Microphone.End(_device);
        double time = _startTime.Subtract(DateTime.Now).TotalSeconds;

        int n = (int) (time + 1) * 44100 ;

        float[] samples = new float[n];

        _clipRecord.GetData(samples, 0);

        AudioClip newClip = AudioClip.Create("Activity registration", n, _clipRecord.channels, _clipRecord.frequency, false);

        newClip.SetData(samples, 0);

        SavWav.Save("Registration", newClip);

        //TODO Aggiungere chiamata a server per salvataggio clip

        EventManager.TriggerEvent("Completed");
    }

    
    //get data from microphone into audioclip
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0)
        {
            return 0;
        }
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }


    private IEnumerator MicrophoneCheck()
    {
        while(true)
        {
            // levelMax equals to the highest normalized value power 2, a small number because < 1
            // pass the value to a static var so we can access it from anywhere
            MicLoudness = LevelMax();
            
            if (MicLoudness > 0.1f)
            {
                Debug.Log("Loudness: " + MicLoudness);
                EventManager.TriggerEvent("SpeechDetected");
            }
            yield return null;
        }
    }

    // start mic when scene starts
    
    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopCoroutine("MicrophoneCheck");
        if(Microphone.IsRecording(_device))
            Microphone.End(_device);
    }

    void OnDestroy()
    {
        StopCoroutine("MicrophoneCheck");
        if(Microphone.IsRecording(_device))
            Microphone.End(_device);
    }
    
}
