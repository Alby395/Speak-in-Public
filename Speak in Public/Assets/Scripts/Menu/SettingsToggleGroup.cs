using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggleGroup: MonoBehaviour
{
    [SerializeField] private string parameter;

    private ToggleGroup _group;

    private void Start()
    {

        _group = GetComponent<ToggleGroup>();

        string lastChoice = PlayerPrefs.GetString(parameter, "");          

        foreach(Toggle toggle in transform.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(delegate {SaveChoice(toggle);});
        }


        if(!lastChoice.Equals(""))
        {
            Transform child = _group.transform.Find(lastChoice);
            
            if(child != null)
                child.GetComponent<Toggle>().isOn = true;
            else
            {
                Toggle toggle = _group.GetComponentInChildren<Toggle>();
                toggle.isOn = true;
                SaveChoice(toggle);
            }
        }
    }

    private void SaveChoice(Toggle toggle)
    {
        print(parameter);
        if(toggle.isOn)
        {
            PlayerPrefs.SetString(parameter, toggle.name);
            PlayerPrefs.Save();
        }
    }
}