using UnityEngine;
using UnityEngine.Android;

public class Menu : MonoBehaviour
{

    private void Start()
    {
        #if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        #endif
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

    public void EnableTWB(bool enable)
    {
        GameManager.instance.TWBenabled = enable;
    }

}