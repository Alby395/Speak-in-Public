using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class MainCanvas : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Settings;

    public TMP_InputField sessionId;

    private void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        Settings.SetActive(false);
    }

    public void ShowSettings()
    {
        MainMenu.SetActive(false);
        Settings.SetActive(true);
    }

    public void StartConnection()
    {
        int session = int.Parse(sessionId.text);
        WebSocketManager.instance.StartWebSocket(session);
    }

    public void StartGame()
    {
        GameManager.instance.LoadLevel();
    }

    public void QuitApp()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    //TEMP
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
