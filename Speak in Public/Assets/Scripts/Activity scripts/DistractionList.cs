using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DistractionList", menuName = "ScriptableObject/DistractionList", order = 1)]
public class DistractionList: ScriptableObject
{
    public Distraction[] distractions;
}

[Serializable]
public class Distraction
{
    public string name;
    public AudioClip audio;
}