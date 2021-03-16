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
        SceneManager.LoadScene(gameId);
    }

    public void GoToMenu()
    {
        gameId = 0;
        SceneManager.LoadScene(gameId);
        WebSocketManager.instance.StopWebSocket();
    }

}
