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
            animator.SetLayerWeight(GameObject.Find("Settings").GetComponent<Settings>().ClassroomDifficulty, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
