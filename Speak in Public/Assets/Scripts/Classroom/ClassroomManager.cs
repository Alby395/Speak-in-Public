using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ClassroomManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator[] people = transform.GetComponentsInChildren<Animator>();
        System.Random rnd = new System.Random();
        Animator[] peopleRnd = people.OrderBy(x => rnd.Next()).ToArray();
        for (int i = 0; i < 8 - GameObject.Find("Settings").GetComponent<Settings>().NumberOfPeople; i++)
        {
            Destroy(peopleRnd[i].gameObject);
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
}
