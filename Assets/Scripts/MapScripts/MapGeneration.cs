using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private List<TileSO> _tiles;
    [SerializeField] private int x,y;

    private MapMatrix<TileSO> _matrix;

    // Start is called before the first frame update
    void Start()
    {
        Random.seed = 23;
        _matrix = new MapMatrix<TileSO>(x, y, _tiles);

        while (_matrix.AreAllTilesSet())
        {
            (int i,int j) tileCords = _matrix.PickRandomTile();
            _matrix.PickTileValue(tileCords.i, tileCords.j);
            _matrix.RemoveImposiblePairs();
        }
        
        for(int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                var result = _matrix.GetTile(i, j);
                if (result != null)
                {
                    Tile tile = result.tile;
                    _tilemap.SetTile(new Vector3Int(i, -j, 0), tile);
                }
                    
            }
        }
        
    }


}
