using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartActivity : MonoBehaviour
{
    private void Awake()
    {
        transform.GetChild(GameManager.instance.TWBenabled == false ? 0 : 1).gameObject.SetActive(true);
    }
}
