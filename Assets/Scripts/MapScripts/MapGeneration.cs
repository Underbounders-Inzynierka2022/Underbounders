using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap _tilesTilemap;
    [SerializeField] private Tilemap _obstaclesTilemap;
    [SerializeField] private Grid _layout;
    [SerializeField] private List<TileSO> _tiles;
    [SerializeField] private List<ObstacleSO> _obstacles;
    [SerializeField] private int x, y;

    private MapMatrix<TileSO> _matriOfTiles;
    private MapMatrix<ObstacleSO> _matriOfObstacles;


    // Start is called before the first frame update
    void Start()
    {
        Random.seed = 23;
        _matriOfTiles = new MapMatrix<TileSO>(x, y, _tiles);

        while (_matriOfTiles.AreAllTilesSet())
        {
            (int i, int j) tileCords = _matriOfTiles.PickRandomTile();
            _matriOfTiles.PickTileValue(tileCords.i, tileCords.j);
            _matriOfTiles.RemoveImposiblePairs();
        }

        List<Dictionary<ObstacleSO, float>> initialObstacles = new List<Dictionary<ObstacleSO, float>>();
        var emptyListObstacles = new List<KeyValuePair<ObstacleSO, float>>() { new KeyValuePair<ObstacleSO, float>(_obstacles.First(y => y.obstacleType == ObstacleType.Empty), 1.0f) };


        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (_matriOfTiles.GetTile(i, j) != null)
                {
                    var dic = _matriOfTiles.GetTile(i, j)?.secondLayerRestrictions;
                    initialObstacles.Add(dic.ToDictionary(x => _obstacles.First(y => y.obstacleType == x.obstacle), x => x.chance));
                }
                else
                {
                    initialObstacles.Add(new Dictionary<ObstacleSO, float>(emptyListObstacles));
                }

                
            }
        }

        _matriOfObstacles = new MapMatrix<ObstacleSO>(x, y, initialObstacles);

        while (_matriOfObstacles.AreAllTilesSet())
        {
            (int i, int j) tileCords = _matriOfObstacles.PickRandomTile();
            _matriOfObstacles.PickTileValue(tileCords.i, tileCords.j);
            _matriOfObstacles.RemoveImposiblePairs();
        }



        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                var result = _matriOfTiles.GetTile(i, j);
                if (result != null)
                {
                    Tile tile = result.tile;
                    _tilesTilemap.SetTile(new Vector3Int(i, -j, 0), tile);
                }
                var obstacleResult = _matriOfObstacles.GetTile(i, j);
                if (obstacleResult != null)
                {
                    if (obstacleResult.tile is Tile obstacleTile)
                        _obstaclesTilemap.SetTile(new Vector3Int(i, -j, 0), obstacleTile);
                    if (obstacleResult.tile is GameObject obstacleObject && obstacleResult.obstacleType != ObstacleType.Empty)
                    {
                        var position = _layout.CellToWorld(new Vector3Int(i+1, -j+1, 0));
                        Instantiate(obstacleObject, position, Quaternion.identity);
                    }
                }
            }
        }

    }


}
