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

    public TileKindName KindOfTile;

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
            float chance = tileToFilter[tile];
            switch (dir)
            {
                case Direction.down:

                    if (!downSide.sideRestrictions.Contains(tile.upSide.SideName))
                        chance = 0;
                    break;
                case Direction.right:
                    if (!rightSide.sideRestrictions.Contains(tile.leftSide.SideName))
                        chance = 0;
                    break;
                case Direction.left:
                    if (!leftSide.sideRestrictions.Contains(tile.rightSide.SideName))
                        chance = 0;
                    break;
                case Direction.up:
                    if (!upSide.sideRestrictions.Contains(tile.downSide.SideName))
                        chance = 0;
                    break;
                case Direction.upRight:
                    if (!upRightCorner.sideRestrictions.Contains(tile.downLeftCorner.SideName))
                        chance = 0;
                    break;
                case Direction.upLeft:
                    if (!upLeftCorner.sideRestrictions.Contains(tile.downRightCorner.SideName))
                        chance = 0;
                    break;
                case Direction.downLeft:
                    if (!downLeftCorner.sideRestrictions.Contains(tile.upRightCorner.SideName))
                        chance = 0;
                    break;
                case Direction.downRight:
                    if (!downRightCorner.sideRestrictions.Contains(tile.upLeftCorner.SideName))
                        chance = 0;
                    break;
            }
            filtered.Add(tile, chance);
        }
        tileToFilter = filtered;
        return filtered;
    }
}

