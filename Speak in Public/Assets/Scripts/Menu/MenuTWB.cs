using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuTWB : Menu
{
    public TMP_InputField sessionId;

    public GameObject StartButton;
    private Button startButton;

    [SerializeField] private Button _connectButton;

    void Awake()
    {
        startButton = StartButton.GetComponent<Button>();

        _connectButton.onClick.AddListener(StartConnection);
        startButton.onClick.AddListener(GameManager.instance.LoadLevel);
    }

    void Update()
    {
        startButton.interactable = WebSocketManager.instance.GetStatus() == WebSocketSharp.WebSocketState.Open;
    }

    public void StartConnection()
    {
        print("Starting connection");

        int session = int.Parse(sessionId.text);
        WebSocketManager.instance.StartWebSocket(session);
    }
}
