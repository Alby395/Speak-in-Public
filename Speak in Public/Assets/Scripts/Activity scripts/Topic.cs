using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Topic : MonoBehaviour
{
    private TMP_Text topic;

    private void Awake()
    {
        topic = GetComponent<TMP_Text>();
              
        if(GameManager.instance.TWBenabled)
        {
            EventManager.StartListening("UpdateTopic", UpdateTopic);
            topic.text = "";
        }
        else
            topic.text = PlayerPrefs.GetString("Topic");
    }

    private void UpdateTopic(string txt)
    {
        topic.text = txt;
    }
}
