using UnityEngine;

public class PlayerBeat : MonoBehaviour
{
    public Sprite[] spriteArray;
    
    private SpriteRenderer _playerSprite;
    private int _spriteCounter;

    void Start()
    {
        _playerSprite = GetComponent <SpriteRenderer>();
        GameEvents.Current.ONBeatHit += OnBeatHit;
    }

    private void OnBeatHit()
    {
        if (_spriteCounter < 3) _spriteCounter++;
        else _spriteCounter = 0;
        _playerSprite.sprite = spriteArray[_spriteCounter];
    }
}
