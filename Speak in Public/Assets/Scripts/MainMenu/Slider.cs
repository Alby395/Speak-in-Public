using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(UnityEngine.UI.Slider))]
public class Slider : MonoBehaviour
{
    public TextMeshProUGUI lab;
    public int defaultValue;
    private UnityEngine.UI.Slider slider;

    private void Awake()
    {

        slider = GetComponent<UnityEngine.UI.Slider>();
        slider.value = PlayerPrefs.GetInt(name, defaultValue);
        lab.text = slider.value.ToString();
        slider.onValueChanged.AddListener(ValueChange);
    }

    private void ValueChange(float val)
    {
        PlayerPrefs.SetInt(name, Mathf.FloorToInt(val));
        PlayerPrefs.Save();
        lab.text = val.ToString();
    }



}
