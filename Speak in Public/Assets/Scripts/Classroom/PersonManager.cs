using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("StartCheering", Cheer);
        EventManager.StartListening("SpeechDetected", DetectSpeech);
    }

    private void OnDestroy()
    {
        EventManager.StopListening("StopCheering", Cheer);
    }

    void DetectSpeech()
    {
        animator.SetTrigger("SpeechDetected");
    }

    void Cheer()
    {
        animator.SetTrigger("Cheer");
    }
}
