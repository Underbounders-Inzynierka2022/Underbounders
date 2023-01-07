using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Representation of singular floor tile, descripting its sides
/// </summary>
[CreateAssetMenu]
public class TileSO : ScriptableObject, ITileKind<TileSO>
{
    /// <summary>
    /// Tile it is representing
    /// </summary>
    public Tile tile;

    /// <summary>
    /// Upside descriptor
    /// </summary>
    [Header("Side description")]
    public TileSideDescriptionsSO upSide;
    /// <summary>
    /// Left descriptor
    /// </summary>
    public TileSideDescriptionsSO leftSide;
    /// <summary>
    /// Right descriptor
    /// </summary>
    public TileSideDescriptionsSO rightSide;
    /// <summary>
    /// Down descriptor
    /// </summary>
    public TileSideDescriptionsSO downSide;

    /// <summary>
    /// Upper left corner descriptor
    /// </summary>
    [Header("Corner description")]
    public TileSideDescriptionsSO upLeftCorner;
    /// <summary>
    /// Upper right corner descriptor
    /// </summary>
    public TileSideDescriptionsSO upRightCorner;
    /// <summary>
    /// Down left corner descriptor
    /// </summary>
    public TileSideDescriptionsSO downLeftCorner;
    /// <summary>
    /// Down right corner descriptor
    /// </summary>
    public TileSideDescriptionsSO downRightCorner;

    /// <summary>
    /// Layer above restrictions
    /// </summary>
    [Header("Second layer restrictions")]
    public List<ObstacleDictionaryPair> secondLayerRestrictions;

    /// <summary>
    /// Monsters spawn restrictions
    /// </summary>
    public List<MonsterDictionaryPair> spawnableMonsters;

    /// <summary>
    /// Filters floor tiles in its particullar neighbourhood
    /// </summary>
    /// <param name="tileToFilter">
    /// Tiles possibilities that needs filtering
    /// </param>
    /// <param name="dir">
    /// Floor tile adjency
    /// </param>
    /// <returns>
    /// Filtered tiles possiblities
    /// </returns>
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
