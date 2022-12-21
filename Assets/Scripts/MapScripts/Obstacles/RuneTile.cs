using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RuneTile : Tile
{
    [SerializeField] private Sprite _offRune;
    [SerializeField] private Sprite _onRune;



    public void TurnTileOn()
    {
        this.sprite = _onRune;
    }

    public void TurnTileOff()
    {
        this.sprite = _offRune;
    }
}
