using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Monster scriptable object to hadle their distributions in the room
/// </summary>
[CreateAssetMenu]
public class MonsterSO : ScriptableObject, ITileKind<MonsterSO>
{
    /// <summary>
    /// Monster represented by the tile
    /// </summary>
    public GameObject Monster;

    /// <summary>
    /// Monster type of the tile
    /// </summary>
    public MonsterType MonsterType;

    /// <summary>
    /// Restrictions to the upward adjacent tile
    /// </summary>
    public List<MonsterDictionaryPair> UpRestrictions;
    /// <summary>
    /// Restrictions to the downward adjacent tile
    /// </summary>
    public List<MonsterDictionaryPair> DownRestrictions;
    /// <summary>
    /// Restrictions to the left adjacent tile
    /// </summary>
    public List<MonsterDictionaryPair> LeftRestrictions;
    /// <summary>
    /// Restrictions to the left adjacent tile
    /// </summary>
    public List<MonsterDictionaryPair> RightRestrictions;

    /// <summary>
    /// Tile filtering method changing propability in the certain tiles
    /// </summary>
    /// <param name="tilesToFilter">
    /// Monsters to filter in the neighbourhood
    /// </param>
    /// <param name="dir">
    /// Direction of adjency
    /// </param>
    /// <returns>
    /// Filtered list of monsters in the adjencent tile
    /// </returns>
    public Dictionary<MonsterSO, float> FilterTiles(Dictionary<MonsterSO, float> tilesToFilter, Direction dir)
    {
        Dictionary<MonsterSO, float> filtered = new Dictionary<MonsterSO, float>();
        foreach (var tile in tilesToFilter.Keys)
        {
            switch (dir)
            {
                case Direction.up:
                    if (DownRestrictions.Any(r => r.monster == tile.MonsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.MonsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.down:
                    if (UpRestrictions.Any(r => r.monster == tile.MonsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.MonsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.left:
                    if (RightRestrictions.Any(r => r.monster == tile.MonsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.MonsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.right:
                    if (LeftRestrictions.Any(r => r.monster == tile.MonsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.MonsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
            }
        }
        tilesToFilter = filtered;
        return filtered;
    }

}
