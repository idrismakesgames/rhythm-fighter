using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Current;
    
    private void Awake()
    {
        Current = this;
    }
    
    public event Action ONBeatHit;
    
    public void BeatHit()
    {
        ONBeatHit?.Invoke();
    }
}
