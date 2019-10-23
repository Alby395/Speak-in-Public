using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class StandaloneMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Settings;

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

    public void StartGame()
    {
        GameManager.instance.gameId = 1;
        GameManager.instance.LoadLevel();
    }

    public void QuitApp()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

}
