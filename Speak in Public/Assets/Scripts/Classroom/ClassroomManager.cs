using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ClassroomManager : MonoBehaviour
{
    private List<Animator> people;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();
        people = transform.GetComponentsInChildren<Animator>().OrderBy(x => rnd.Next()).ToList();
        for (int i = 0; i < 8 - GameObject.Find("Settings").GetComponent<Settings>().NumberOfPeople; i++)
        {
            Destroy(people[i].gameObject);
            people.RemoveAt(i);
        }

        foreach (Animator animator in people)
        {
            animator.SetBool(animator.GetParameter(GameObject.Find("Settings").GetComponent<Settings>().ClassroomDifficulty).nameHash, true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DetectSpeech()
    {
        foreach (Animator animator in people)
        {
            animator.SetTrigger("SpeechDetected");
        }
    }
}
