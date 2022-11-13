using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileSO : ScriptableObject, ITileKind<TileSO>
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

    public Dictionary<TileSO, float> FilterTiles(Dictionary<TileSO, float> tileToFilter, Direction dir)
    {
        Dictionary<TileSO, float> filtered = new Dictionary<TileSO, float>();
        foreach (var tile in tileToFilter.Keys)
        {
            switch (dir)
            {
                case Direction.down:
                    if (upRestrictions.Any(r => r.kindOfSide == tile.downSideDescription))
                    {
                        float chance =  upRestrictions.Where(r => r.kindOfSide == tile.downSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }  
                    break;
                case Direction.rigth:
                    if (leftRestrictions.Any(r => r.kindOfSide == tile.rightSideDescription))
                    {
                        float chance = leftRestrictions.Where(r => r.kindOfSide == tile.rightSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.left:
                    if (upRestrictions.Any(r => r.kindOfSide == tile.leftSideDescription))
                    {
                        float chance = rightRestrictions.Where(r => r.kindOfSide == tile.leftSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.up:
                    if (downRestrictions.Any(r => r.kindOfSide == tile.upSideDescription))
                    {
                        float chance = downRestrictions.Where(r => r.kindOfSide == tile.upSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
            }
        }
        return filtered;
    }
}
