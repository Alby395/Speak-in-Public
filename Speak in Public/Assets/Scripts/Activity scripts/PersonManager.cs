using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    public bool priority;
    public bool distractible;

    private Animator[] animators;
    private Coroutine _cor;

    private bool isBusy;

    private void Awake()
    {
        animators = transform.GetComponentsInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.TWBenabled)
        {
            EventManager.StartListening("Reset", Reset);
        }

        EventManager.StartListening("StartCheering", Cheer);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("StartCheering", Cheer);

        if (GameManager.instance.TWBenabled)
        {
            EventManager.StopListening("Reset", Reset);
        }
    }

    public void StartDistraction()
    {
            EventManager.StartListening("SpeechDetected", DistractStandalone);
    }
    
    void DistractStandalone()
    {
        if(_cor != null)
            StopCoroutine(_cor);

        _cor = StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        //Debug.Log((float)new System.Random().NextDouble());
        yield return new WaitForSeconds((float)new System.Random().NextDouble());

        Distract();
    }


    
    void Cheer()
    {
        foreach(Animator anim in animators)
            anim.SetTrigger("Cheers");
    }

    public void DistractTWB()
    {
        EventManager.StartListening("Reset", Reset);

        Distract();
    }

    private void Distract()
    {
        int rng = new System.Random().Next(0,animators[0].GetInteger("NumberOfStates"));
        foreach(Animator anim in animators)
        {
            anim.SetInteger("Random", rng);
            anim.SetTrigger("Distract");
        }
    }

    private void Reset()
    {
        foreach(Animator anim in animators)
        {
            anim.SetTrigger("Idle");
        }

        EventManager.StopListening("Reset", Reset);
    }
}
