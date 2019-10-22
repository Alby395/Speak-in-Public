using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{

    private Button bottone;

    private void Awake()
    {
        bottone = GetComponent<Button>();
    }

    private void Update()
    {
        //bottone.interactable = WebSocketManager.instance.GetStatus() == WebSocketSharp.WebSocketState.Open;
    }


}
