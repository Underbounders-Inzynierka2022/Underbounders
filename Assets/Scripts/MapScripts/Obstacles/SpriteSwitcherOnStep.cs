using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteSwitcherOnStep : MonoBehaviour
{
    [SerializeField] private Sprite _offRune;
    [SerializeField] private Sprite _onRune;
    [SerializeField] private SpriteRenderer _renderer;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Triggered");
        _renderer.sprite = _onRune;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _renderer.sprite = _offRune;
    }
}
