using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Toggle))]
public class SettingsToggle : MonoBehaviour
{
    public string parameter;
    private UnityEngine.UI.Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<UnityEngine.UI.Toggle>();
        toggle.isOn = PlayerPrefs.GetInt(parameter) == 0 ? false : true;
        toggle.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(bool val)
    {
        PlayerPrefs.SetInt(parameter, val == false ? 0 : 1);
        PlayerPrefs.Save();
    }
}