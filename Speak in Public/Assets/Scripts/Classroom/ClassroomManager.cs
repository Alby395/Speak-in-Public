using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.XR;
using UnityEditor;

public class ClassroomManager : MonoBehaviour
{
    public int NumberOfPeople;
    public float PercentageOfDistractedPeople;
    public bool MicrophoneEnabled;

    public GameObject people;
    private List<Animator> animators;

    private void Awake()
    {
        if (!EditorApplication.isPlaying)
        {
            NumberOfPeople = PlayerPrefs.GetInt("NumberOfPeople");
            PercentageOfDistractedPeople = Mathf.FloorToInt(PlayerPrefs.GetInt("NumberOfPeople") * PlayerPrefs.GetInt("PercentageOfDistractedPeople"));
            MicrophoneEnabled = (PlayerPrefs.GetInt("MicrophoneEnabled") == 0) ? false : true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();
        animators = people.GetComponentsInChildren<Animator>().OrderBy(x => rnd.Next()).ToList();
        for (int i = 0; i < 8 - NumberOfPeople; i++)
        {
            Destroy(animators[i].gameObject);
            animators.RemoveAt(i);
        }

        foreach (Animator animator in animators)
        {
            animator.SetBool("Difficult", true);
        }

        StartCoroutine(LoadDevice("cardboard", true));
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DetectSpeech()
    {
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("SpeechDetected");
        }
    }

    private IEnumerator LoadDevice(string newDevice, bool enable)
    {
        if (string.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = enable;
        }
    }
}
