﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;

[RequireComponent(typeof(Camera))]
public class Streaminig : MonoBehaviour
{
    private Camera virtuCamera;
    private RenderTexture rendTexture;
    //private Texture2D streaming;
    private readonly int width = 480;
    private readonly int height = 360;
    private Coroutine stream;

    private int quality;
    private int fps;

    private void Awake()
    {
        /*
        virtuCamera = Camera.main;

        rendTexture = new RenderTexture(width, height, 24);
        */

        //streaming = new Texture2D(width, height, TextureFormat.RGB24, false);
        
        //virtuCamera.aspect = width / height;

        quality = PlayerPrefs.GetInt("QualityStreaming", 50);
        fps = 24;
    }

    private void Start()
    {
        if (GameManager.instance.TWBenabled)
        {
            stream = StartCoroutine(SendStreaming());
        }
        else
        {
            this.enabled = false;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance.TWBenabled)
        {
            StopCoroutine(stream);
        }
    }

    IEnumerator SendStreaming()
    {
        float rate = 1f / (float)fps;

        while (WebSocketManager.instance.GetStatus() == WebSocketState.Open)
        {
            yield return new WaitForEndOfFrame();
            SendFrame(false);
            yield return new WaitForSeconds(rate);
        }
    }

    public void SendFrame(bool important)
    {
        /*
        virtuCamera.Render();
        virtuCamera.targetTexture = rendTexture;
        RenderTexture.active = rendTexture;
        streaming.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        streaming.Apply();
        RenderTexture.active = null;
        virtuCamera.targetTexture = null;
        */
        Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        
        tex.Resize(width, height);
        tex.Apply(false);
        
        string frame = Convert.ToBase64String(tex.EncodeToJPG(quality));

        Destroy(tex);

        Streaming message = new Streaming();
        message.important = important;
        message.data = new Data();
        message.data.frame = "data:image/jpg; base64," + frame;
        message.data.width = width;
        message.data.height = height;

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

        // TODO aggiungere rotazione - Quaternion.toEuler
    }



}
