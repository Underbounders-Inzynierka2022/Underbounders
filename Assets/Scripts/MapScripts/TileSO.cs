using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileSO : ScriptableObject
{
    public Tile tile;
    public TileType tileId;

    public SideDescription upSideDescription;
    public SideDescription rightSideDescription;
    public SideDescription downSideDescription;
    public SideDescription leftSideDescription;

    public List<TileDictionaryPair> upRestrictions;
    public List<TileDictionaryPair> rightRestrictions;
    public List<TileDictionaryPair> downRestrictions;
    public List<TileDictionaryPair> leftRestrictions;

    public Dictionary<ObstacleType, float> secondLayerRestrictions;

    public bool spawnable;

    public Dictionary<string, float> spawnableMosnters;
}
