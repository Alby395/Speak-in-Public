﻿using System.Collections;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.Networking;
using System.IO;
using System.Collections.Generic;
using System.Threading;

public class WebSocketManager : MonoBehaviour
{
    [SerializeField] private TextAsset networkConfigurationFile;

    private int idSession;
    private int activityId;
    private int _audioId;

    private WebSocket ws;
    private string url;
    private string urlReply;


    public static WebSocketManager instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void StartWebSocket(int idSession)
    {
        NetworkConfiguration config = JsonUtility.FromJson<NetworkConfiguration>(networkConfigurationFile.text);

        url = config.url;
        urlReply = config.urlReply;
       
        this.idSession = idSession;
        if (url != null && urlReply != null)
        {
            StartCoroutine(GetActivityID());
        }

    }

    public void StopWebSocket()
    {
        if(ws != null)
        {
            ws.Close();
            ws = null;
        }
    }

    IEnumerator GetActivityID()
    {
        string urlTWB = "https://" + url + "/configuration/" + idSession;
        UnityWebRequest webRequest = UnityWebRequest.Get(urlTWB);
        
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error: " + webRequest.error);
            yield break;
        }

        string res = webRequest.downloadHandler.text;

        activityId = JsonUtility.FromJson<Response>(res).id;
        
        string urlReply = "https://" + url + "/act/" + activityId;

        UnityWebRequest webRequestReply = UnityWebRequest.Get(urlReply);

        yield return webRequestReply.SendWebRequest();

        if (webRequestReply.isNetworkError)
        {
            Debug.Log("Error Reply: " + webRequestReply.error);
            yield break;
        }

        string resReply = webRequestReply.downloadHandler.text;

        JsonUtility.FromJson<ResponseReply>(resReply).configuration.Setup();
        
        StartWebSocket();
    }

    private void StartWebSocket()
    {
        ws = new WebSocket("wss://" + url + "/activity?activity=" + activityId + "&id=" + idSession);
        ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Chiusura WS");
            print(e.Reason);
            ws.Close();
        };
        ws.OnError += (sender, e) =>
        {
            Debug.Log("Errore WS");
            Debug.Log(e.Exception);
            ws = null;
        };
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Aperto WS");
        };
        ws.Connect();
    }
    
    public void SendAudio(Thread ts)
    {
        StartCoroutine(SendAudioCoroutine(ts));
    }

    public void SetAudioId(int id)
    {
        _audioId = id;
    }

    private IEnumerator SendAudioCoroutine(Thread ts)
    {
        string urlAudio = "https://" + url + "/audio/" + _audioId;

        while(ts.IsAlive)
        {
            yield return null;
        }

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("audio", File.ReadAllBytes(Application.persistentDataPath + "/Registration.wav"), "Registration.wav", "audio/wav"));
        yield return null;
        
        UnityWebRequest request = UnityWebRequest.Post(urlAudio, formData);

        print("Sending audio");
        yield return request.SendWebRequest();
        print("Audio sent");


        EventManager.TriggerEvent("Completed");

        Destroy(this.gameObject);

    }

    public WebSocketState GetStatus()
    {
        return ws != null ? ws.ReadyState : WebSocketState.Closed;
    }

    public void SendMessageWeb(string msg)
    {
        if(ws.ReadyState == WebSocketState.Open)
        {
            ws.Send(msg);
        }
    }

    public void AddHandlerMessage(EventHandler<MessageEventArgs> handler)
    {
        if(ws != null)
        {
            print("Aggiungo Handler");
            ws.OnMessage += handler;
        }
    }

    public void RemoveHandlerMessage(EventHandler<MessageEventArgs> handler)
    {
        if (ws != null)
        {
            ws.OnMessage -= handler;
        }
    }



    [Serializable]
    private class Response
    {
        public int id;
    }

    [Serializable]
    private class ResponseReply
    {
        public Configuration configuration;
    }

    [Serializable]
    private class Configuration
    {
        public string oculus_scene;
        public int timer;
        public void Setup()
        {
            PlayerPrefs.SetInt("MicrophoneEnabled", 1);
            PlayerPrefs.SetString("Topic", "");
            PlayerPrefs.SetString("Location", oculus_scene);
            print("time: " + timer);
            PlayerPrefs.SetInt("ActivityDuration", timer);
            PlayerPrefs.Save();
        }
    }
}

[Serializable]
public class ConfigurationDetail
{
    public string oculusScene;     //TODO Aggiungere caricamento scena corretta
    public string Location;     //TODO controllare vero nome di questo campo

    public void Setup()
    {
        PlayerPrefs.SetInt("MicrophoneEnabled", 1);
        PlayerPrefs.SetString("Topic", "");
        PlayerPrefs.SetString("Location", oculusScene);
        PlayerPrefs.Save();
    }
}

[Serializable]
public class NetworkConfiguration
{
    public string url;
    public string urlReply;
}