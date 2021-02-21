using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    private TMP_InputField _currentInput;
    private Canvas _canvas;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
    }

    public void ShowKeyboard(TMP_InputField input)
    {
        _currentInput = input;
        _canvas.enabled = true;
    }

    public void Delete()
    {
        if(_currentInput.text.Length > 0)
            _currentInput.text = _currentInput.text.Substring(0, _currentInput.text.Length - 1);
    }

    public void Write(TMP_Text txt)
    {
        _currentInput.text += txt.text;
    }

    public void HideKeyboard()
    {
        _canvas.enabled = false;
    }
}
