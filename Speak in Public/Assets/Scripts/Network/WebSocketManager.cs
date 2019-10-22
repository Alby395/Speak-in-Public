using System.Collections;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.Networking;

public class WebSocketManager : MonoBehaviour
{
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
        url = PlayerPrefs.GetString("URL", null);
        urlReply = PlayerPrefs.GetString("URLReply", null);
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
        string urlTWB = "http://" + url + "/configuration/" + idSession;
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
        UnityWebRequest webRequestReply = UnityWebRequest.Get(urlReply + activityId);
        yield return webRequestReply.SendWebRequest();
        if (webRequestReply.isNetworkError)
        {
            Debug.Log("Error Reply: " + webRequestReply.error);
            yield break;
        }
        string resReply = webRequestReply.downloadHandler.text;
        Debug.Log("Received: " + resReply);
        GameManager.instance.gameId = JsonUtility.FromJson<ResponseReply>(resReply).configuration.configuration.id_activity;
        StartWebSocket();
    }

    private void StartWebSocket()
    {
        ws = new WebSocket("ws://" + url + "/activity?activity=" + activityId + "&id=" + idSession);
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("Chiusura WS");
        };
        ws.OnError += (sender, e) =>
        {
            Debug.Log("Errore WS");
            Debug.Log(e.Exception);
        };
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Aperto WS");
        };
        ws.Connect();
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

    [Serializable]
    private class ConfigurationDetail
    {
        public int id_activity;
    }

}
