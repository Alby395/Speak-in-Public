using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Toggle))]
public class SettingsToggle : MonoBehaviour
{
    public string parameter;
    public TextMeshProUGUI value;
    public int defaultValue;
    private UnityEngine.UI.Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<UnityEngine.UI.Toggle>();
        toggle.isOn = PlayerPrefs.GetInt(name, defaultValue) == 0 ? false : true;
        toggle.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(bool val)
    {
        PlayerPrefs.SetInt(name, val == false ? 0 : 1);
        PlayerPrefs.Save();
    }
}