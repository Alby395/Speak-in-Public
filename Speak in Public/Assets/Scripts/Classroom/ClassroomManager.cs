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

public abstract class ClassroomManager : MonoBehaviour
{
    private int NumberOfPeople;
    private float PercentageOfDistractedPeople;
    private bool MicrophoneEnabled;

    public GameObject TextTopic;
    public GameObject people;
    private List<Animator> animators;
    

    public static ClassroomManager instance
    {
        get;
        private set;
    }

    public virtual void Awake()
    {
        NumberOfPeople = PlayerPrefs.GetInt("NumberOfPeople");
        PercentageOfDistractedPeople = Mathf.FloorToInt(PlayerPrefs.GetInt("NumberOfPeople") * PlayerPrefs.GetInt("PercentageOfDistractedPeople") / 100);
        MicrophoneEnabled = (PlayerPrefs.GetInt("MicrophoneEnabled") == 0) ? false : true;
    }

    // Start is called before the first frame update
    public virtual void Start()
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

        transform.parent.GetComponent<MicrophoneManager>().enabled = MicrophoneEnabled;


        Init();
    }

    protected abstract void Init();

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.GoToMenu();
        }
    }


    protected virtual IEnumerator ActivityTerminated()
    {
        yield return new WaitForSeconds(10f);
        GameManager.instance.GoToMenu();
    }

}


