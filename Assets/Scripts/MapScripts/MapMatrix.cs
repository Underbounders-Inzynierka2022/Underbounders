using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Matrix using wave function collapse to be resolved
/// </summary>
/// <typeparam name="T">
/// Tile reference type of possibility group, it has to implement <see cref="ITileKind{T}"/>
/// </typeparam>
public class MapMatrix<T> where T : ITileKind<T>
{
    /// <summary>
    /// Number of collumns
    /// </summary>
    private int x;

    /// <summary>
    /// Number of rows
    /// </summary>
    private int y;

    /// <summary>
    /// Matrix containing tiles possibilities
    /// </summary>
    private Dictionary<T, float>[,] map;

    /// <summary>
    /// This constructor creates matrix with equal number of equally propable possibilities
    /// </summary>
    /// <param name="x">
    /// Number of collumns in the matrix
    /// </param>
    /// <param name="y">
    /// Number of rows in the matrix
    /// </param>
    /// <param name="intializator">
    /// Collection of tiles possibilities
    /// </param>
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

    /// <summary>
    /// This constructor creates matrix with unequal primary state
    /// </summary>
    /// <param name="x">
    /// Number of collumns in the matrix
    /// </param>
    /// <param name="y">
    /// Number of rows in the matrix
    /// </param>
    /// <param name="initialValues">
    /// List of tiles with its possibilities and its chances
    /// </param>
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
    /// <summary>
    /// Picks random tile from map that has lowest, greater than 0, enthropy
    /// </summary>
    /// <returns>
    /// Position of the chosen tile in the matrix
    /// </returns>
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
    /// <summary>
    /// Randomly chooses particullar tiles possibility as its only value
    /// </summary>
    /// <param name="i">
    /// Collumn of tile
    /// </param>
    /// <param name="j">
    /// Row of tile
    /// </param>
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
    /// <summary>
    /// Searches for lowest, greater than 0, enthropy tiles from matrix
    /// </summary>
    /// <returns>
    /// List of lowest, greater than 0, enthropy tiles from matrix
    /// </returns>
    public List<Dictionary<T, float>> GetLowestCountList()
    {
        var list = map.Cast<Dictionary<T, float>>().ToList();
        int min = list.Select(ts => ts.Count).Where(c => c > 1).Min();
        return list.Where(ts => ts.Count == min).ToList();
    }
    /// <summary>
    /// Update tiles around particular tile
    /// </summary>
    /// <param name="i">
    /// Collumn of the tile
    /// </param>
    /// <param name="j">
    /// Row of the tile
    /// </param>
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
    /// <summary>
    /// Removes impossible candidats per tile from matrix
    /// </summary>
    public void RemoveImposiblePairs()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                List<T> impossiblekeys = map[i, j].Where(t => t.Value <= 0f).Select(t => t.Key).ToList();
                foreach (var key in impossiblekeys)
                    map[i, j].Remove(key);
                if (map[i, j].Count == 1 && impossiblekeys.Count > 0) UpdateTilesAround(i, j);
            }
        }
    }
    /// <summary>
    /// Checks if there is enthropy equal 0 or contradiction occurs on every tile of the whole matrix 
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if there is enthropy equal 0 or contradiction occurs on every tile of the whole matrix, <see langword="false"/> otherwise
    /// </returns>
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

    /// <summary>
    /// Returns particular tile from matrix
    /// </summary>
    /// <param name="i">
    /// Column of the tile
    /// </param>
    /// <param name="j">
    /// Row of the tile
    /// </param>
    /// <returns>
    /// Tile <see cref="T"/> from particular cell if exists, default <see cref="T"/> value otherwise
    /// </returns>
    public T GetTile(int i, int j)
    {
        if (i >= 0 && j >= 0 && i < x && j < y)
        {
            var result = map[i, j];
            return result.FirstOrDefault().Key;
        }

        return default(T);
    }

    /// <summary>
    /// Resolves matrix with wave function collapse algorythm
    /// </summary>
    public void ResolveMatrix()
    {
        while (AreAllTilesSet())
        {
            (int i, int j) tileCords = PickRandomTile();
            PickTileValue(tileCords.i, tileCords.j);
            RemoveImposiblePairs();
        }
    }
}
