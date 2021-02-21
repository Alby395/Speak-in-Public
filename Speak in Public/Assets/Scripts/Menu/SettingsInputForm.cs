using System;
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

        input.onSelect.AddListener(ShowKeyboard);
    }

    public void SavePref()
    {
        PlayerPrefs.SetString(namePref, input.text);
        PlayerPrefs.Save();
    }

    private void ShowKeyboard(string str)
    {
        GameObject.Find("/KeyboardCanvas").GetComponent<Keyboard>().ShowKeyboard(input);
    }
}
