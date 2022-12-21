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

    [Header("Side description")]
    public TileSideDescriptionsSO upSide;
    public TileSideDescriptionsSO leftSide;
    public TileSideDescriptionsSO rightSide;
    public TileSideDescriptionsSO downSide;


    [Header("Corner description")]
    public TileSideDescriptionsSO upLeftCorner;
    public TileSideDescriptionsSO upRightCorner;
    public TileSideDescriptionsSO downLeftCorner;
    public TileSideDescriptionsSO downRightCorner;

    [Header("Second layer restrictions")]
    public List<ObstacleDictionaryPair> secondLayerRestrictions;

    public List<MonsterDictionaryPair> spawnableMonsters;

    public Dictionary<TileSO, float> FilterTiles(Dictionary<TileSO, float> tileToFilter, Direction dir)
    {
        //if (tileToFilter.Count == 1) return tileToFilter;
        Dictionary<TileSO, float> filtered = new Dictionary<TileSO, float>();
        foreach (var tile in tileToFilter.Keys)
        {
            switch (dir)
            {
                case Direction.down:
                    if (downSide.sideRestrictions.Any(r => r.kindOfSide == tile.upSide.SideName))
                    {
                        float chance = downSide.sideRestrictions.Where(r => r.kindOfSide == tile.upSide.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.right:
                    if (rightSide.sideRestrictions.Any(r => r.kindOfSide == tile.leftSide.SideName))
                    {
                        float chance = rightSide.sideRestrictions.Where(r => r.kindOfSide == tile.leftSide.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.left:
                    if (leftSide.sideRestrictions.Any(r => r.kindOfSide == tile.rightSide.SideName))
                    {
                        float chance = leftSide.sideRestrictions.Where(r => r.kindOfSide == tile.rightSide.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.up:
                    if (upSide.sideRestrictions.Any(r => r.kindOfSide == tile.downSide.SideName))
                    {
                        float chance = upSide.sideRestrictions.Where(r => r.kindOfSide == tile.downSide.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.upRight:
                    if (upRightCorner.sideRestrictions.Any(r => r.kindOfSide == tile.downLeftCorner.SideName))
                    {
                        float chance = upRightCorner.sideRestrictions.Where(r => r.kindOfSide == tile.downLeftCorner.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.upLeft:
                    if (upLeftCorner.sideRestrictions.Any(r => r.kindOfSide == tile.downRightCorner.SideName))
                    {
                        float chance = upLeftCorner.sideRestrictions.Where(r => r.kindOfSide == tile.downRightCorner.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.downLeft:
                    if (downLeftCorner.sideRestrictions.Any(r => r.kindOfSide == tile.upRightCorner.SideName))
                    {
                        float chance = downLeftCorner.sideRestrictions.Where(r => r.kindOfSide == tile.upRightCorner.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
                case Direction.downRight:
                    if (downRightCorner.sideRestrictions.Any(r => r.kindOfSide == tile.upLeftCorner.SideName))
                    {
                        float chance = downRightCorner.sideRestrictions.Where(r => r.kindOfSide == tile.upLeftCorner.SideName).First().chance;
                        filtered.Add(tile, tileToFilter[tile] * chance);
                    }
                    break;
            }
        }
        return filtered;
    }
}
