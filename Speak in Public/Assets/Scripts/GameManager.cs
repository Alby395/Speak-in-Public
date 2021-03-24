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
            Application.targetFrameRate = 45;
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
		string scene = PlayerPrefs.GetString("Location", "");

        switch(scene)
        {
            case "School":
                GameManager.instance.gameId = 1;
                break;
            
            case "Restaurant":
                GameManager.instance.gameId = 2;
                break;
            
            case "Doctor's office":
                GameManager.instance.gameId = 3;
                break;

            default:
                GameManager.instance.gameId = 1;
                break;
        }
		
        SceneManager.LoadSceneAsync(gameId);
    }

    public void GoToMenu()
    {
        gameId = 0;
        SceneManager.LoadSceneAsync(gameId);
        WebSocketManager.instance.StopWebSocket();
    }

}
