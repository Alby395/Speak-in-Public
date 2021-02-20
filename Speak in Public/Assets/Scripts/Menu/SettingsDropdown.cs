using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDropdown: MonoBehaviour
{
    [SerializeField] private string parameter;

    private TMP_Dropdown _dropdown;

    private void Start()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        string lastChoice = PlayerPrefs.GetString(parameter, "");

        int i = 0;
        print(_dropdown.options.Count);

        while(i < _dropdown.options.Count && !_dropdown.options[i].text.Equals(lastChoice))
        {
            i++;
        }

        if(i < _dropdown.options.Count)
            _dropdown.value = i;
        else
            PlayerPrefs.SetString(parameter, _dropdown.options[0].text);

        _dropdown.onValueChanged.AddListener(SaveChoice);
    }

    private void SaveChoice(int choice)
    {
        print(_dropdown.options[choice].text);

        PlayerPrefs.SetString(parameter, _dropdown.options[choice].text);
        PlayerPrefs.Save();
    }
}