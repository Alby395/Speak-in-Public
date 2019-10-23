using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Slider))]
public class SettingsSlider : MonoBehaviour
{
    public string parameter;
    public TextMeshProUGUI value;
    public int defaultValue;
    private UnityEngine.UI.Slider slider;

    private void Awake()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();
        slider.value = PlayerPrefs.GetInt(name, defaultValue);
        value.text = slider.value.ToString();
        slider.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(float val)
    {
        PlayerPrefs.SetInt(name, Mathf.FloorToInt(val));
        PlayerPrefs.Save();
        value.text = val.ToString();
    }
}