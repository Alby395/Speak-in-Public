using System.Collections;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class WebSocketManager : MonoBehaviour
{
    public Text debug;

    [SerializeField] private TextAsset networkConfigurationFile;

    private int idSession;
    private int activityId;
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
        debug.text = "HERE\n";
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
        debug.text += "\nConnecting";
        string urlTWB = "https://" + url + "configuration/" + idSession;
        UnityWebRequest webRequest = UnityWebRequest.Get(urlTWB);
        Debug.Log(urlTWB);
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError)
        {
            Debug.Log("Error: " + webRequest.error);
            yield break;
        }

        string res = webRequest.downloadHandler.text;
        Debug.Log("Received: " + res);
        

        activityId = JsonUtility.FromJson<Response>(res).id;
        /*
        UnityWebRequest webRequestReply = UnityWebRequest.Get(Path.Combine(urlReply + activityId));

        yield return webRequestReply.SendWebRequest();

        if (webRequestReply.isNetworkError)
        {
            Debug.Log("Error Reply: " + webRequestReply.error);
            yield break;
        }

        string resReply = webRequestReply.downloadHandler.text;
        Debug.Log("Received: " + resReply);

        ConfigurationDetail det = JsonUtility.FromJson<ResponseReply>(resReply).configuration.configuration;
        GameManager.instance.gameId = det.id_activity;  //TODO Aggiungere caricamento scena corretta

        det.Setup();
        */
        PlayerPrefs.SetInt("NumberOfPeople", 8);
        PlayerPrefs.SetInt("PercentageOfDistractedPeople", 0);
        PlayerPrefs.SetInt("MicrophoneEnabled", 0);
        PlayerPrefs.Save();

        StartWebSocket();
    }

    private void StartWebSocket()
    {
        ws = new WebSocket("wss://" + url + "activity?activity=" + activityId + "&id=" + idSession);
        ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

        print("wss://" + url + "activity?activity=" + activityId + "&id=" + idSession);
        debug.text += "\nwss://" + url + "activity?activity=" + activityId + "&id=" + idSession + "\n";
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Chiusura WS");
            print(e.Reason);
        };
        ws.OnError += (sender, e) =>
        {
            Debug.Log("Errore WS");
            Debug.Log(e.Exception);
        };
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Aperto WS");
            debug.text += "Aperto";
        };
        ws.Connect();
    }
    
    public void SendAudio()
    {
        StartCoroutine(SendAudioCoroutine());
        
    }

    private IEnumerator SendAudioCoroutine()
    {
        string urlAudio = url; //TODO completare
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("audio", File.ReadAllBytes(Application.persistentDataPath + "/Registration.wav")));
        yield return null;

        UnityWebRequest request = UnityWebRequest.Post(urlAudio, formData);
        yield return request.SendWebRequest();

        print(request.downloadHandler.text);

        EventManager.TriggerEvent("Completed");
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
        public int id;
        public string value;
        public string category;
        public string description;
        public bool active;
        public Configuration configuration;
    }

    [Serializable]
    private class Configuration
    {
        public string name;
        public string description;
        public string category;
        public string img;
        public string url;
        public int device_id;
        public ConfigurationDetail configuration;
    }

   

}

[Serializable]
public class ConfigurationDetail
{
    public int NumberOfPeople;
    public float PercentageOfDistractedPeople;
    public bool MicrophoneEnabled;
    public int id_activity;     //TODO Aggiungere caricamento scena corretta
    public string Topic;

    public void Setup()
    {
        PlayerPrefs.SetInt("NumberOfPeople", NumberOfPeople);
        PlayerPrefs.SetInt("PercentageOfDistractedPeople", Mathf.FloorToInt(PercentageOfDistractedPeople));
        PlayerPrefs.SetInt("MicrophoneEnabled", MicrophoneEnabled ? 1 : 0);
        PlayerPrefs.SetString("Topic", Topic);
        PlayerPrefs.Save();
    }
}

[Serializable]
public class NetworkConfiguration
{
    public string url;
    public string urlReply;
}