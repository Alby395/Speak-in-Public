using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.XR;

public class ClassroomManager : MonoBehaviour
{
    private List<Animator> people;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();
        people = transform.GetComponentsInChildren<Animator>().OrderBy(x => rnd.Next()).ToList();
        for (int i = 0; i < 8 - GameObject.Find("ActivitySettings").GetComponent<Settings>().NumberOfPeople; i++)
        {
            Destroy(people[i].gameObject);
            people.RemoveAt(i);
        }

        foreach (Animator animator in people)
        {
            animator.SetBool(animator.GetParameter((int)GameObject.Find("ActivitySettings").GetComponent<Settings>().PercentageOfDistractedPeople).nameHash, true);
        }

        StartCoroutine(LoadDevice("cardboard", true));
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void DetectSpeech()
    {
        foreach (Animator animator in people)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).ToString() != "Sitting Idle")
            {
                animator.SetTrigger("SpeechDetected");
            }
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
