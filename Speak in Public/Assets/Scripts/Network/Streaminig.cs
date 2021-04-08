using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WebSocketSharp;
using UnityEngine.Rendering;
using System.Threading;

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
        fps = 8;
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
            
            StartCoroutine(ManageFrame(false));
            yield return new WaitForSeconds(rate);
        }
    }

    public IEnumerator ManageFrame(bool important)
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
        
        //Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        
        /*
        yield return new WaitForEndOfFrame();

        RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
        ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);

        AsyncGPUReadback.Request(renderTexture, 0, TextureFormat.RGBA32, req => StartCoroutine(ResizeFrame(req)));
        RenderTexture.ReleaseTemporary(renderTexture);
        */
        RenderTexture rtt = RenderTexture.GetTemporary(Screen.width, Screen.height);
        /*
        Camera.main.targetTexture = rtt;
        RenderTexture.active = rtt;

        Texture2D tex = new Texture2D(width, height);

        int posX = (Screen.width - width)/2;
        int posY = (Screen.height - height)/2;
        yield return new WaitForEndOfFrame();
        */
        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshotIntoRenderTexture(rtt);

        Texture2D tex = new Texture2D(width, height);

        RenderTexture.active = rtt;

        int posX = (Screen.width - width)/2;
        int posY = (Screen.height - height)/2;
        tex.ReadPixels(new Rect(posX, posY, width, height), 0, 0, false);

        tex.Apply(false);

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rtt);
        yield return null;

        string frame = Convert.ToBase64String(tex.EncodeToJPG(quality));

        Thread ts = new Thread(new ThreadStart(() => {
            Streaming message = new Streaming();
            message.important = important;
            message.data = new Data();
            message.data.frame = "data:image/jpg; base64," + frame;
            message.data.width = width;
            message.data.height = height;

            WebSocketManager.instance.SendMessageWeb(JsonUtility.ToJson(message));
        }));
        ts.Start();

        RenderTexture.ReleaseTemporary(rtt);
        DestroyImmediate(tex);

        /*
        yield return null;
        
        TextureScaler.scale(tex, width, height, FilterMode.Point);

        yield return null;
        */
    }

    private IEnumerator ResizeFrame(AsyncGPUReadbackRequest req)
    {
        if(req.hasError)
        {
            print("ERROR GPU");
            yield break;
        }

        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);

        tex.LoadRawTextureData(req.GetData<uint>());
        
        yield return null;
        
        TextureScaler.scale(tex, width, height, FilterMode.Point);
        yield return null;

        string frame = Convert.ToBase64String(tex.EncodeToJPG(quality));
        
        DestroyImmediate(tex);

        yield return null;
        Streaming message = new Streaming();
        message.important = false;
        message.data = new Data();
        message.data.frame = "data:image/jpg; base64," + frame;
        message.data.width = width;
        message.data.height = height;

        WebSocketManager.instance.SendMessageWeb(JsonUtility.ToJson(message));    
    }

private Texture2D ScaleTexture(Texture2D source,int targetWidth,int targetHeight) {
    Texture2D result=new Texture2D(targetWidth,targetHeight,source.format,true);
	Color[] rpixels=result.GetPixels(0);
	float incX=(1.0f / (float)targetWidth);
	float incY=(1.0f / (float)targetHeight); 
	for(int px=0; px<rpixels.Length; px++) { 
		rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth))); 
	} 
	result.SetPixels(rpixels,0); 
	result.Apply(); 
	return result; 
}
    private void SendFrame(AsyncGPUReadbackRequest req)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        string frame = Convert.ToBase64String(tex.EncodeToJPG(quality));

        DestroyImmediate(tex);

        Streaming message = new Streaming();
        message.important = false;
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
