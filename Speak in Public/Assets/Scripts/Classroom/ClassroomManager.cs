using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.XR;
using UnityEditor;

public class ClassroomManager : MonoBehaviour
{
    private int NumberOfPeople;
    private float PercentageOfDistractedPeople;
    private bool MicrophoneEnabled;

    public GameObject people;
    private List<Animator> animators;

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

        if (MicrophoneEnabled)
        {
            transform.GetComponent<MicrophoneManager>().enabled = true;
        }
    }


    public void DetectSpeech()
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("SpeechDetected");
        }
    }


    [Serializable]
    private class Message
    {
        public string type;
        public string message;
    }
}
