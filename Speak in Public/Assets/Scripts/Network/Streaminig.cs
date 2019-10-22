using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;

[RequireComponent(typeof(Camera))]
public class Streaminig : MonoBehaviour
{
    private Camera virtuCamera;
    private RenderTexture rendTexture;
    private Texture2D streaming;
    private readonly int width = 480;
    private readonly int height = 360;
    private Coroutine stream;

    private int quality;
    private int fps;

    private void Awake()
    {
        virtuCamera = GetComponent<Camera>();
        rendTexture = new RenderTexture(width, height, 24);
        streaming = new Texture2D(width, height, TextureFormat.RGB24, false);
        virtuCamera.aspect = width / height;
        virtuCamera.targetTexture = rendTexture;
        quality = PlayerPrefs.GetInt("QualityStreaming", 50);
        fps = PlayerPrefs.GetInt("RateStreaming", 50);
    }

    private void Start()
    {
        stream = StartCoroutine(SendStreaming());
    }

    private void Update()
    {
        gameObject.SetActive(WebSocketManager.instance.GetStatus() == WebSocketState.Open);
    }

    private void OnDestroy()
    {
        StopCoroutine(stream);
    }

    IEnumerator SendStreaming()
    {
        float rate = 1 / (float)fps;
        while (true)
        {
           //SendFrame(false, LevelManager.instance.attention);
           yield return new WaitForSeconds(rate);
        }
    }

    public void SendFrame(bool important, int value)
    {
        virtuCamera.Render();
        RenderTexture.active = rendTexture;
        streaming.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        streaming.Apply();
        RenderTexture.active = null;
        string frame = Convert.ToBase64String(streaming.EncodeToJPG(quality));
        Streaming message = new Streaming();
        message.important = important;
        message.data = new Data();
        message.data.frame = "data:image/jpg; base64," + frame;
        message.data.width = width;
        message.data.height = height;
        message.data.@params = new Params();
        message.data.@params.value = new Value();
        message.data.@params.value.attention = value;
        WebSocketManager.instance.SendMessageWeb(JsonUtility.ToJson(message));
    }

    [Serializable]
    private class Streaming
    {
        public bool important;
        public Data data;
    }

    [Serializable]
    private class Data
    {
        public string frame;
        public int width;
        public int height;
        public Params @params;
    }

    [Serializable]
    private class Params
    {
        public Value value;
    }

    [Serializable]
    private class Value
    {
        public int attention;
    }


}
