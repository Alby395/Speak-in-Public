using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DistractionManager : MonoBehaviour
{
    [SerializeField] private Distraction[] distractions;
    [SerializeField] private GameObject darkBox;
    [SerializeField] private float lightTime;

    private Dictionary<string, AudioClip> audioDistraction;
    private AudioSource _audio;
    private Material _mat;
    private float _alpha;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();

        audioDistraction = new Dictionary<string, AudioClip>();

        if(distractions.Length > 0)
        {
            foreach(Distraction dis in distractions)
            {
                audioDistraction.Add(dis.name, dis.audio);
            }

            EventManager.StartListening("PlayDistraction", PlayAudio);
        }
        
        EventManager.StartListening("Lights", TurnOff);

        _mat = darkBox.GetComponent<MeshRenderer>().material;
        _alpha = _mat.color.a;

        Color col = _mat.color;
        col.a = 0f;
        _mat.color = col;
    }

    private void PlayAudio(string audio)
    {
        AudioClip audioClip;

        if(audioDistraction.TryGetValue(audio, out audioClip))
        {
            _audio.PlayOneShot(audioClip);
        }
    }

    private void TurnOff()
    {
        StartCoroutine(ChangeMaterial(true));
        EventManager.StartListening("Lights", TurnOn);
        EventManager.StopListening("Lights", TurnOff);
    }

    private void TurnOn()
    {
        StartCoroutine(ChangeMaterial(false));
        EventManager.StartListening("Lights", TurnOff);
        EventManager.StopListening("Lights", TurnOn);
    }

    private IEnumerator ChangeMaterial(bool dark)
    {
        float time = 0;
        darkBox.gameObject.SetActive(true);

        while(time < lightTime)
        {
            time += Time.deltaTime;

            Color col = _mat.color;
            col.a = _alpha * (dark? time/lightTime : 1 - time/lightTime);

            _mat.color = col;

            yield return null;
        }
    }
}

[Serializable]
public class Distraction
{
    public string name;
    public AudioClip audio;
}
