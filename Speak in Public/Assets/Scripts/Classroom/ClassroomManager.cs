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
            animator.SetBool(animator.GetParameter(GameObject.Find("Settings").GetComponent<Settings>().ClassroomDifficulty).nameHash, true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
