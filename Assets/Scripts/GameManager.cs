using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float audioBpm = 120f;
    public int beatInterval = 1;
    public float audioLatency = 0.02f;
    public int startDelay = 30;
    
    private AudioSource _audioTheme;
    private float _interval8;
    private float _beatCounter = -0.00f; 
    private float _remainder;
    private bool _firstTimeAddition;
    private int _flashCounter;

    void Start()
    {
        _audioTheme = GetComponent<AudioSource>();
        _interval8 = ((60f / audioBpm) / 8);
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (startDelay > 0)
        {
            startDelay--;
            if (startDelay == 0) _audioTheme.Play(0);
        } 
        else
        {
            if (!_firstTimeAddition) CheckStartDelay();
            IncreaseBeatCounter();
            if (_remainder < Time.deltaTime) FlashBeat();
        }
    }

    #region Rhythm / Beat Methods
    void CheckStartDelay()
    {
        _beatCounter -= _audioTheme.time + audioLatency;
        _firstTimeAddition = true;
    }

    void IncreaseBeatCounter()
    {
        _beatCounter += Time.deltaTime;
        _remainder = _interval8 - _beatCounter;
    }

    void FlashBeat()
    {
        _beatCounter = 0 - _remainder;

        if (_flashCounter < 7) _flashCounter += 1;
        else _flashCounter = 0;

        if ((beatInterval == 1 && _flashCounter == 7)
            || (beatInterval == 2 && (_flashCounter == 7 || _flashCounter == 3))
            || (beatInterval == 4 && (_flashCounter % 2 == 1))
            || (beatInterval == 8))
        {
            GameEvents.Current.BeatHit();
        }
    }
    #endregion
}
    