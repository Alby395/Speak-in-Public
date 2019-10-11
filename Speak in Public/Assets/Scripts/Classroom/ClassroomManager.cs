using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassroomManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator[] animators = transform.GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            animator.GetParameter(GameObject.Find("Settings").GetComponent<Settings>().ClassroomDifficulty).defaultBool = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
