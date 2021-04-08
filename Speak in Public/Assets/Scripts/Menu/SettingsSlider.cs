using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Slider))]
public class SettingsSlider : MonoBehaviour
{
    public string parameter;
    public TextMeshProUGUI value;
    private UnityEngine.UI.Slider slider;

    private void Awake()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();

        int val = PlayerPrefs.GetInt(parameter, -1);
        
        if(val != -1)
        {
            slider.value = PlayerPrefs.GetInt(parameter, -1);
        }

        value.text = slider.value.ToString();
        slider.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(float val)
    {
        PlayerPrefs.SetInt(parameter, Mathf.FloorToInt(val));
        PlayerPrefs.Save();
        value.text = val.ToString();
    }
}