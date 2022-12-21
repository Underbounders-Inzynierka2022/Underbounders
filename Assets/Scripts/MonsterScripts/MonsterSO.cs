using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class MonsterSO : ScriptableObject, ITileKind<MonsterSO>
{
    public GameObject monster;

    public MonsterType monsterType;

    public List<MonsterDictionaryPair> UpRestrictions;
    public List<MonsterDictionaryPair> DownRestrictions;
    public List<MonsterDictionaryPair> LeftRestrictions;
    public List<MonsterDictionaryPair> RightRestrictions;

    public Dictionary<MonsterSO, float> FilterTiles(Dictionary<MonsterSO, float> tilesToFilter, Direction dir)
    {
        Dictionary<MonsterSO, float> filtered = new Dictionary<MonsterSO, float>();
        foreach (var tile in tilesToFilter.Keys)
        {
            switch (dir)
            {
                case Direction.up:
                    if (DownRestrictions.Any(r => r.monster == tile.monsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.monsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.down:
                    if (UpRestrictions.Any(r => r.monster == tile.monsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.monsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.left:
                    if (RightRestrictions.Any(r => r.monster == tile.monsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.monsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
                case Direction.right:
                    if (LeftRestrictions.Any(r => r.monster == tile.monsterType))
                    {
                        float chance = DownRestrictions.Where(r => r.monster == tile.monsterType).First().chance;
                        filtered.Add(tile, tilesToFilter[tile] * chance);
                    }
                    break;
            }
        }
        return filtered;
    }

}
