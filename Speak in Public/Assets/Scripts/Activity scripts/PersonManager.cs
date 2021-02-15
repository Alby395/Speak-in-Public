using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    private Animator animator;

    private bool isBusy;

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
        EventManager.StopListening("SpeechDetected", DetectSpeech);
    }

    void DetectSpeech()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        //Debug.Log((float)new System.Random().NextDouble());
        yield return new WaitForSeconds((float)new System.Random().NextDouble());
        animator.SetTrigger("SpeechDetected");
    }

    void Cheer()
    {
        animator.SetTrigger("StartCheering");
    }
}
