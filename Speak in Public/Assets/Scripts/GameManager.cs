using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public int gameId;

    public bool TWBenabled;


	public static GameManager instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
            gameId = 0;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

    public void LoadLevel()
    {
        SceneManager.LoadScene(gameId);
    }

    public void GoToMenu()
    {
        gameId = 0;
        SceneManager.LoadScene(gameId);
        WebSocketManager.instance.StopWebSocket();
        DisableVR();
    }

    private IEnumerator LoadDevice(string newDevice, bool enable)
    {

        if (string.Compare(XRSettings.loadedDeviceName, newDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(newDevice);
            yield return null;
            XRSettings.enabled = enable;
            if (string.IsNullOrEmpty(newDevice))
            {
                Application.targetFrameRate = -1;
            }
            else
            {
                Application.targetFrameRate = 60;
                QualitySettings.vSyncCount = 0;
            }
        }
        
    }

    public void EnableVR()
    {
        StartCoroutine(LoadDevice("cardboard", true));
    }

    public void DisableVR()
    {
        StartCoroutine(LoadDevice("", false));
    }

}
