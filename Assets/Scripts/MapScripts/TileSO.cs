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

    public SideDescription upLeftCornerDescription;
    public SideDescription upRightCornerDescription;
    public SideDescription downLeftCornerDescription;
    public SideDescription downRightCornerDescription;

    public List<TileDictionaryPair> upRestrictions;
    public List<TileDictionaryPair> rightRestrictions;
    public List<TileDictionaryPair> downRestrictions;
    public List<TileDictionaryPair> leftRestrictions;

    public List<TileDictionaryPair> upLeftRestrictions;
    public List<TileDictionaryPair> upRightRestrictions;
    public List<TileDictionaryPair> downLeftRestrictions;
    public List<TileDictionaryPair> downRightRestrictions;

    public Dictionary<ObstacleType, float> secondLayerRestrictions;

    public bool spawnable;

    public Dictionary<string, float> spawnableMosnters;

    public Dictionary<TileSO, float> FilterTiles(Dictionary<TileSO, float> tileToFilter, Direction dir)
    {
        //if (tileToFilter.Count == 1) return tileToFilter;
        Dictionary<TileSO, float> filtered = new Dictionary<TileSO, float>();
        foreach (var tile in tileToFilter.Keys)
        {
            switch (dir)
            {
                case Direction.down:
                    if (downRestrictions.Any(r => r.kindOfSide == tile.upSideDescription))
                    {
                        float chance = downRestrictions.Where(r => r.kindOfSide == tile.upSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.right:
                    if (rightRestrictions.Any(r => r.kindOfSide == tile.leftSideDescription))
                    {
                        float chance = rightRestrictions.Where(r => r.kindOfSide == tile.leftSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.left:
                    if (leftRestrictions.Any(r => r.kindOfSide == tile.rightSideDescription))
                    {
                        float chance = leftRestrictions.Where(r => r.kindOfSide == tile.rightSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.up:
                    if (upRestrictions.Any(r => r.kindOfSide == tile.downSideDescription))
                    {
                        float chance = upRestrictions.Where(r => r.kindOfSide == tile.downSideDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.upRight:
                    if (upRightRestrictions.Any(r => r.kindOfSide == tile.downLeftCornerDescription))
                    {
                        float chance = upRightRestrictions.Where(r => r.kindOfSide == tile.downLeftCornerDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.upLeft:
                    if (upLeftRestrictions.Any(r => r.kindOfSide == tile.downRightCornerDescription))
                    {
                        float chance = upLeftRestrictions.Where(r => r.kindOfSide == tile.downRightCornerDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.downLeft:
                    if (downLeftRestrictions.Any(r => r.kindOfSide == tile.upRightCornerDescription))
                    {
                        float chance = downLeftRestrictions.Where(r => r.kindOfSide == tile.upRightCornerDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.downRight:
                    if (downRightRestrictions.Any(r => r.kindOfSide == tile.upLeftCornerDescription))
                    {
                        float chance = downRightRestrictions.Where(r => r.kindOfSide == tile.upLeftCornerDescription).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
            }
        }
        return filtered;
    }
}
