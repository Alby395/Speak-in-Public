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
        topic.text = PlayerPrefs.GetString("Topic");
        EventManager.StartListening("UpdateTopic", UpdateTopic);
    }

    private void UpdateTopic()
    {
        topic.text = PlayerPrefs.GetString("Topic");
    }
}
