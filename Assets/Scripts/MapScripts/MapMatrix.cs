using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapMatrix<T>
{
    public int x { set; get; }

    public int y { set; get; }

    private List<Dictionary<T, float>> map;

    MapMatrix(int x, int y, List<T> intializator)
    {
        map = new List<Dictionary<T, float>>();
        float initialChance = 1 / intializator.Count;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                map[i * y + j] = new Dictionary<T, float>();
                foreach (var tile in intializator)
                {
                    map[i * y + j].Add(tile, initialChance);
                }
            }
        }
    }

    Dictionary<T, float> PickRandomTile()
    {
        var lowestList = GetLowestCountList();
        return lowestList[Random.Range(0, lowestList.Count - 1)];
    }

    T PickTileValue(Dictionary<T, float> tile)
    {
        int t = HelperFunctions.RandomWeighted(tile.Values.ToList());
        return tile.Keys.ToList()[t];
    }

    List<Dictionary<T, float>> GetLowestCountList()
    {
        int min = map.Select(x => x.Count).Where(x => x > 1).Min();
        return map.Where(x => x.Count == min).ToList();
    }
}
