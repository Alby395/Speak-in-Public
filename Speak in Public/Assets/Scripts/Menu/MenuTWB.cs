using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuTWB : Menu
{
    public Text debug;

    public TMP_InputField sessionId;

    public GameObject StartButton;
    private Button startButton;

    void Awake()
    {
        startButton = StartButton.GetComponent<Button>();
    }

    void Update()
    {
        startButton.interactable = WebSocketManager.instance.GetStatus() == WebSocketSharp.WebSocketState.Open;
    }

    public void StartConnection()
    {
        debug.text = "pressed";
        int session = int.Parse(sessionId.text);
        WebSocketManager.instance.StartWebSocket(session);
    }
}
