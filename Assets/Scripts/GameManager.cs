using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float audioBpm = 120f;
    public int beatInterval = 1;
    public GameObject player;
    public Sprite[] spriteArray;

    private AudioSource _audioTheme;
    private SpriteRenderer _playerSprite;
    private float _interval8;

    private float _beatCounter = -0.00f; // Add latency delay. Will need Latency Tester.
    private float _remainder;
    private bool _firstTimeAddition;
    private int _flashCounter;
    private int _spriteCounter;

    void Start()
    {
        _audioTheme = GetComponent<AudioSource>();
        _playerSprite = player.GetComponent<SpriteRenderer>();
        _interval8 = ((60f / audioBpm) / 8);

        Application.targetFrameRate = 60;
        _audioTheme.Play(0);
    }

    void Update()
    {
        if (!_firstTimeAddition) CheckStartDelay();
        IncreaseBeatCounter();
        if (_remainder < Time.deltaTime) FlashBeat();
    }

    #region Rhythm / Beat Methods
    void CheckStartDelay()
    {
        _beatCounter -= _audioTheme.time;
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
            CyclePlayerSprite();
        }
    }

    void CyclePlayerSprite()
    {
        if (_spriteCounter < 3) _spriteCounter++;
        else _spriteCounter = 0;
        
        _playerSprite.sprite = spriteArray[_spriteCounter];
    }
    #endregion
}
    