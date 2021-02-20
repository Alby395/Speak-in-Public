using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class SettingsInputForm : MonoBehaviour
{
    public string namePref;
    private TMP_InputField input;

    private void Start()
    {
        input = GetComponent<TMP_InputField>();
        input.text = PlayerPrefs.GetString(namePref, "");
    }

    public void SavePref()
    {
        PlayerPrefs.SetString(namePref, input.text);
        PlayerPrefs.Save();
    }
}
