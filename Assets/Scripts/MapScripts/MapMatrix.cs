using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapMatrix<T> where T : ITileKind<T>
{
    private int x;

    private int y;

    private Dictionary<T, float>[,] map;

    public MapMatrix(int x, int y, IEnumerable<T> intializator)
    {
        this.x = x;
        this.y = y;
        map = new Dictionary<T, float>[x, y];
        float initialChance = 100 / intializator.Count();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                map[i, j] = new Dictionary<T, float>();
                foreach (var tile in intializator)
                {
                    map[i, j].Add(tile, initialChance);
                }
            }
        }
    }

    public MapMatrix(int x, int y, List<Dictionary<T,float>> initialValues)
    {
        this.x = x;
        this.y = y;
        map = new Dictionary<T, float>[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                map[i, j] = initialValues.First().ToDictionary(x => x.Key, x => x.Value);
                initialValues.RemoveAt(0);
            }
        }
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                UpdateTilesAround(i, j);
                RemoveImposiblePairs();
            }
        }
    }

    public (int, int) PickRandomTile()
    {
        var lowestList = GetLowestCountList();
        var dictionary = lowestList[UnityEngine.Random.Range(0, lowestList.Count - 1)];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (map[i, j] == dictionary)
                    return (i, j);
            }
        }
        return (-1, -1);
    }

    public void PickTileValue(int i, int j)
    {
        if (i >= 0 && i < x && j >= 0 && j <= y)
        {
            int chosenTile = HelperFunctions.RandomWeighted(map[i, j].Values.ToList());
            var chosenTileKind = map[i, j].ToList()[chosenTile];
            map[i, j] = new Dictionary<T, float>();
            map[i, j].Add(chosenTileKind.Key, 1f);
           UpdateTilesAround(i, j);
        }
    }

    public List<Dictionary<T, float>> GetLowestCountList()
    {
        var list = map.Cast<Dictionary<T, float>>().ToList();
        int min = list.Select(ts => ts.Count).Where(c => c > 1).Min();
        return list.Where(ts => ts.Count == min).ToList();
    }

    private void UpdateTilesAround(int i, int j)
    {

        if (map[i, j].Count == 1)
        {
            if (j > 0)
                map[i, j - 1] = map[i, j].First().Key.FilterTiles(map[i, j - 1], Direction.up);
            if (j < y - 1)
                map[i, j + 1] = map[i, j].First().Key.FilterTiles(map[i, j + 1], Direction.down);
            if (i > 0)
                map[i - 1, j] = map[i, j].First().Key.FilterTiles(map[i - 1, j], Direction.left);
            if (i < x - 1)
                map[i + 1, j] = map[i, j].First().Key.FilterTiles(map[i + 1, j], Direction.right);
            if (j > 0 && i > 0)
                map[i-1, j - 1] = map[i, j].First().Key.FilterTiles(map[i-1, j - 1], Direction.upLeft);
            if (j < y - 1 && i > 0)
                map[i-1, j + 1] = map[i, j].First().Key.FilterTiles(map[i-1, j + 1], Direction.downLeft);
            if (j > 0 && i < x -1)
                map[i + 1, j - 1] = map[i, j].First().Key.FilterTiles(map[i + 1, j - 1], Direction.upRight);
            if (j < y - 1 && i < x-1)
                map[i + 1, j + 1] = map[i, j].First().Key.FilterTiles(map[i + 1, j + 1], Direction.downRight);
        }

    }

    public void RemoveImposiblePairs()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                List<T> impossiblekeys = map[i, j].Where(t => t.Value <= 0f).Select(t => t.Key).ToList();
                foreach (var key in impossiblekeys)
                    map[i, j].Remove(key);
            }
        }
    }

    public bool AreAllTilesSet()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (map[i, j].Count > 1)
                    return true;
            }
        }
        return false;
    }

    public T GetTile(int i, int j)
    {
        if (i >= 0 && j >= 0 && i < x && j < y)
        {
            var result = map[i, j];
            return result.FirstOrDefault().Key;
        }

        return default(T);
    }
}
