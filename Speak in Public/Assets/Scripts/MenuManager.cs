using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadActivity(int ActivityNumber)
    {
        SceneManager.LoadScene(ActivityNumber);
        if (ActivityNumber != 0)
        {
            StartCoroutine(LoadDevice("daydream", true));
        }
        else
        {
            StartCoroutine(LoadDevice("", false));
        }
    }

    private IEnumerator LoadDevice(string newDevice, bool enable)
    {
        if (string.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return new WaitForEndOfFrame();
            XRSettings.enabled = enable;
        }
    }
}
