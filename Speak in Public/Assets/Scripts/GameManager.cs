using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class GameManager : MonoBehaviour
{
    public int gameId;

    public bool TWBenabled;

    public bool mobile {get; private set;}

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

            //Application.targetFrameRate = 45;
            mobile = SceneManager.GetActiveScene().name.Contains("Mobile");
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
        print("TWB? " + TWBenabled);

		string scene = PlayerPrefs.GetString("Location", "");

        switch(scene)
        {
            case "School":
                GameManager.instance.gameId = 1;
                break;
            
            case "Bar":
                GameManager.instance.gameId = 2;
                break;
            
            case "Doctor": 
                GameManager.instance.gameId = 3;
                break;

            case "Work":
                GameManager.instance.gameId = 4;
                break;
            
            case "OneTable":
                GameManager.instance.gameId = 5;
                break;

            default:
                GameManager.instance.gameId = 1;
                break;
        }

        if(mobile)
        {
            StartCoroutine(LoadXR());
        }
        else
            SceneManager.LoadSceneAsync(gameId);
    }

    private IEnumerator LoadXR()
    {
        AsyncOperation handle = SceneManager.LoadSceneAsync(gameId);
        handle.allowSceneActivation = false;

        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        handle.allowSceneActivation = true;

        XRGeneralSettings.Instance.Manager.StartSubsystems();
    }

    public void GoToMenu()
    {
        TWBenabled = false;
        if(mobile)
        {
            SceneManager.LoadSceneAsync("MenuSceneMobile");
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
        else
        {
            SceneManager.LoadSceneAsync("MenuScene");
        }
        
        WebSocketManager.instance.StopWebSocket();
    }

}
