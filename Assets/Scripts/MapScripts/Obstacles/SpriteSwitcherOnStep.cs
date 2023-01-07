using UnityEngine;

/// <summary>
/// Switches sprites when stepped on by player
/// </summary>
public class SpriteSwitcherOnStep : MonoBehaviour
{
    /// <summary>
    /// Rune sprite in off state
    /// </summary>
    [SerializeField] private Sprite _offRune;
    /// <summary>
    /// Rune sprite if its working
    /// </summary>
    [SerializeField] private Sprite _onRune;
    /// <summary>
    /// Rune sprite renderer
    /// </summary>
    [SerializeField] private SpriteRenderer _renderer;

    void OnTriggerEnter2D(Collider2D col)
    {
       if(col.CompareTag("Player"))
        _renderer.sprite = _onRune;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            _renderer.sprite = _offRune;
    }
}
