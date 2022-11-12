using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private List<TileSO> _tiles;
    [SerializeField] private int x,y;

    // Start is called before the first frame update
    void Start()
    {
        var mapMapping = InitializeArray(x, y);

        
        
    }

    Dictionary<TileType, float>[,] InitializeArray(int x, int y)
    {
        Dictionary<TileType, float>[,] tileMap = new Dictionary<TileType, float>[x, y];
        float initialChance = 1 / _tiles.Count;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                tileMap[i,j] = new Dictionary<TileType, float>();
                foreach(var tile in _tiles)
                {
                    tileMap[i, j].Add(tile.tileId, initialChance);
                }
            }
        }
        return tileMap;
    }

}
