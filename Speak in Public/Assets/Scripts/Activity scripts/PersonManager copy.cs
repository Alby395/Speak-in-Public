﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManagerBefore : MonoBehaviour
{
    public bool priority;
    public bool distractible;

    private Animator[] animators;

    private bool isBusy;

    private void Awake()
    {
        animators = transform.GetComponentsInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("StartCheering", Cheer);
    }

    private void OnDestroy()
    {

        EventManager.StopListening("StartCheering", Cheer);
    }

    public void SetDistract()
    {
        foreach(Animator anim in animators)
            anim.SetBool("Reactive", true);

        if (GameManager.instance.TWBenabled)
        {
            foreach(Animator anim in animators)
                anim.SetInteger("Random", -1);

            EventManager.StartListening("Distract", Distract);
            EventManager.StartListening("Reset", Reset);
        }
        else
        {
            EventManager.StopListening("SpeechDetected", DetectSpeech);
        }
    }

    void DetectSpeech()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        yield return new WaitForSeconds((float)new System.Random().NextDouble());

        Distract();
    }

    void Cheer()
    {
        foreach(Animator anim in animators)
            anim.SetTrigger("StartCheering");
    }

    private void Distract()
    {
        print("distracted");
        int rng = new System.Random().Next(0,animators[0].GetInteger("NumberOfStates"));
        foreach(Animator anim in animators)
        {
            anim.SetTrigger("SpeechDetected");
            anim.SetInteger("Random", rng);
        }
    }

    private void Reset()
    {
        foreach(Animator anim in animators)
        {
            anim.SetTrigger("SpeechDetected");
            anim.SetInteger("Random", -1);
        }
    }
}
