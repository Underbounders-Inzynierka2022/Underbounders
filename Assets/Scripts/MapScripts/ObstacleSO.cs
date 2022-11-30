using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ObstacleSO : ScriptableObject, ITileKind<ObstacleSO>
{
    public UnityEngine.Object tile;

    public ObstacleType obstacleType;

    public List<ObstacleDictionaryPair> UpRestrictions;
    public List<ObstacleDictionaryPair> DownRestrictions;
    public List<ObstacleDictionaryPair> LeftRestrictions;
    public List<ObstacleDictionaryPair> RightRestrictions;

    public bool spawnable;

    public Dictionary<string, string> SpawnRates;

    public Dictionary<ObstacleSO, float> FilterTiles(Dictionary<ObstacleSO, float> tilesToFilter, Direction dir)
    {
        //if (tileToFilter.Count == 1) return tileToFilter;
        Dictionary<ObstacleSO, float> filtered = new Dictionary<ObstacleSO, float>();
        foreach (var tile in tilesToFilter.Keys)
        {
            switch (dir)
            {
                case Direction.down:
                    if (DownRestrictions.Any(r => r.obstacle == tile.obstacleType))
                    {
                        float chance = DownRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.right:
                    if (UpRestrictions.Any(r => r.obstacle == tile.obstacleType))
                    {
                        float chance = RightRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.left:
                    if (RightRestrictions.Any(r => r.obstacle == tile.obstacleType))
                    {
                        float chance = LeftRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.up:
                    if (UpRestrictions.Any(r => r.obstacle == tile.obstacleType))
                    {
                        float chance = UpRestrictions.Where(r => r.obstacle == tile.obstacleType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                default:
                    filtered.Add(tile, tilesToFilter[tile]);
                    break;
            }
        }
        return filtered;
    }

}
