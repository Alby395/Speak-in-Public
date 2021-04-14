using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.Rendering;
using System.Threading;
using System.IO;

[RequireComponent(typeof(Camera))]
public class StreamingWithCamera : MonoBehaviour
{
    private Camera virtuCamera;
    private Transform cameraTransform;

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
        virtuCamera.aspect = width/height;
        cameraTransform = Camera.main.transform;

        rendTexture = new RenderTexture(width, height, 24);
        virtuCamera.targetTexture = rendTexture;

        streaming = new Texture2D(width, height, TextureFormat.RGB24, false);

        quality = PlayerPrefs.GetInt("StreamingQuality", 50);
        fps = PlayerPrefs.GetInt("StreamingRate", 4);

    }

    private void Start()
    {
        if (GameManager.instance.TWBenabled)
        {
            stream = StartCoroutine(SendStreaming());
        }
        else
        {
            gameObject.SetActive(false);
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
            StartCoroutine(ManageFrame(false));
            yield return new WaitForSeconds(rate);
        }
    }

    public IEnumerator ManageFrame(bool important)
    {
        RenderTexture.active = rendTexture;
        virtuCamera.Render();
        
        streaming.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
        streaming.Apply();

        RenderTexture.active = null;

        yield return null;
        string frame = Convert.ToBase64String(streaming.EncodeToJPG(quality));

        Vector3 rot = cameraTransform.localEulerAngles;

        Thread ts = new Thread(new ThreadStart(() => {
            Streaming message = new Streaming();
            message.important = important;
            message.data = new Data();
            message.data.frame = "data:image/jpg; base64," + frame;
            message.data.width = width;
            message.data.height = height;
            message.data.parameters = new CustomParams();
            message.data.parameters.value = new CustomValues(rot);
            
            WebSocketManager.instance.SendMessageWeb(JsonUtility.ToJson(message).Replace("parameters", "params"));
        }));
        ts.Start();      
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

        public CustomParams parameters;
    }

    [Serializable]
    private class CustomParams
    {
        public CustomValues value;

    }
    [Serializable]
    private class CustomValues
    {
        public CustomValues(Vector3 rot)
        {
            rotation_x = rot.x;
            rotation_y = rot.y;
            rotation_z = rot.z;
        }

        public float rotation_x;
        public float rotation_y;
        public float rotation_z;
    }


}
