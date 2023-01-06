using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGeneration : MonoBehaviour
{
    public static MapGeneration instance;
    public bool isDone;
    public int x, y;
    public int seed = 23;
    public List<(int x, int y)> isChestOpen;
    [SerializeField] private Tilemap _tilesTilemap;
    [SerializeField] private Tilemap _obstaclesTilemap;
    [SerializeField] private Tilemap _wallsTilemap;
    [SerializeField] private Grid _layout;
    [SerializeField] private List<TileSO> _tiles;
    [SerializeField] private List<ObstacleSO> _obstacles;
    [SerializeField] private List<MonsterSO> _monsters;

    [SerializeField] private List<TileSO> DoorsTiles;
    [SerializeField] private ObstacleSO EmptyObstacle;
    [SerializeField] private MonsterSO EmptySpawn;
    public bool[] isDoorOnSide;

    private List<GameObject> currObjects;

    [SerializeField] private List<Tile> borderTileUp;
    [SerializeField] private List<Tile> borderTileSide;
    [SerializeField] private List<GameObject> doors;

    private MapMatrix<TileSO> _matrixOfTiles;
    private MapMatrix<ObstacleSO> _matriOfObstacles;
    private MapMatrix<MonsterSO> _matriOfMonsters;

    private void Awake()
    {
        instance = this;
        isDone = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        currObjects = new List<GameObject>();


    }

    public void ClearMap()
    {
        foreach (var objectToDestroy in currObjects)
            Destroy(objectToDestroy);
        _tilesTilemap.ClearAllTiles();
        _obstaclesTilemap.ClearAllTiles();
        _wallsTilemap.ClearAllTiles();
    }

    public void GenerateRoom(int seed, int x, int y)
    {
        this.x = x;
        this.y = y;
        Random.InitState(seed);
        GenerateOutSide();
        GenerateInside();
        isDone = true;

    }

    private void GenerateOutSide()
    {
        SetCorners();
        SetSideWalls();
        SetUpDownWalls();
    }

    private void SetCorners()
    {
        var pos = new Vector3Int(-1, 3, 0);
        _wallsTilemap.SetTile(pos, borderTileSide[1]);
        _wallsTilemap.SetTransformMatrix(pos, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 90f)));
        pos = new Vector3Int(-1, -y, 0);
        _wallsTilemap.SetTile(pos, borderTileSide[1]);
        _wallsTilemap.SetTransformMatrix(pos, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180f)));
        pos = new Vector3Int(x, -y, 0);
        _wallsTilemap.SetTile(pos, borderTileSide[1]);
        _wallsTilemap.SetTransformMatrix(pos, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270f)));
        pos = new Vector3Int(x, 3, 0);
        _wallsTilemap.SetTile(pos, borderTileSide[1]);
        _wallsTilemap.SetTransformMatrix(pos, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 00f)));
    }

    private void SetSideWalls()
    {
        Vector3Int pos;
        for (int i = 2; i > -y; i--)
        {
            if ((i == -y / 2 || i == -y / 2 - 1) && isDoorOnSide[0])
            {
                pos = new Vector3Int(x + 1, i + 1, 0);
                var instance = Instantiate(doors[0], _layout.CellToWorld(pos), Quaternion.Euler(0, 0, 0));
                currObjects.Add(instance);
                instance.GetComponent<DoorsController>().dir = Direction.right;
            }
            else
            {
                pos = new Vector3Int(x, i, 0);
                _wallsTilemap.SetTile(pos, borderTileSide[0]);
            }
            if ((i == -y / 2 || i == -y / 2 - 1) && isDoorOnSide[2])
            {
                pos = new Vector3Int(0, i + 1, 0);
                var instance = Instantiate(doors[0], _layout.CellToWorld(pos), Quaternion.Euler(0, 0, 180f));
                currObjects.Add(instance);
                instance.GetComponent<DoorsController>().dir = Direction.left;
            }
            else
            {
                pos = new Vector3Int(-1, i, 0);
                _wallsTilemap.SetTile(pos, borderTileSide[0]);
                _wallsTilemap.SetTransformMatrix(pos, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180f)));
            }
        }
    }

    private void SetUpDownWalls()
    {
        Vector3Int pos;
        for (int i = 0; i < x; i++)
        {
            if ((i == x / 2 || i == x / 2 + 1) && isDoorOnSide[1])
            {

                pos = new Vector3Int(i + 1, 2, 0);
                var tempPos = _layout.CellToWorld(pos);
                var instance = Instantiate(doors[1], new Vector3(tempPos.x, tempPos.y + 0.08f, tempPos.z), Quaternion.Euler(0, 0, 0));
                currObjects.Add(instance);
                instance.GetComponent<DoorsController>().dir = Direction.up;
            }
            else
            {
                pos = new Vector3Int(i, 3, 0);
                _wallsTilemap.SetTile(pos, borderTileUp[0]);
                pos = new Vector3Int(i, 2, 0);
                _wallsTilemap.SetTile(pos, borderTileUp[1]);
                pos = new Vector3Int(i, 1, 0);
                _wallsTilemap.SetTile(pos, borderTileUp[2]);
            }
            if ((i == x / 2 || i == x / 2 + 1) && isDoorOnSide[3])
            {
                pos = new Vector3Int(i + 1, -y + 1, 0);
                var instance = Instantiate(doors[0], _layout.CellToWorld(pos), Quaternion.Euler(0, 0, 270f));
                currObjects.Add(instance);
                instance.GetComponent<DoorsController>().dir = Direction.down;

            }
            else
            {
                pos = new Vector3Int(i, -y, 0);
                _wallsTilemap.SetTile(pos, borderTileUp[0]);
                _wallsTilemap.SetTransformMatrix(pos, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 180f)));
            }
        }
    }

    private void GenerateInside()
    {
        
        var tilesInit = GetInitialFirstLayer();
        _matrixOfTiles = new MapMatrix<TileSO>(x, y, tilesInit);

        ResolveMatrix(_matrixOfTiles);

        var initialObstacles = GetInitialSecondLayer();
        _matriOfObstacles = new MapMatrix<ObstacleSO>(x, y, initialObstacles);

        ResolveMatrix(_matriOfObstacles);

        var  initialMonsters = GetInitialThirdLayer();

        _matriOfMonsters = new MapMatrix<MonsterSO>(x, y, initialMonsters);

        ResolveMatrix(_matriOfMonsters);

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                var result = _matrixOfTiles.GetTile(i, j);
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
                        var position = _layout.CellToWorld(new Vector3Int(i + 1, -j + 1, 0));
                        var instance = Instantiate(obstacleObject, position, Quaternion.identity);
                        currObjects.Add(instance);
                        
                        var control = instance.GetComponent<ChestController>();
                        if(control != null)
                        {
                            control.chestPos = (i, j);
                            if (isChestOpen != null && isChestOpen.Any() && isChestOpen.Any(t => t.x == i && t.y == j))
                            {
                                    control.ChangeSprite();
                            }
                        }
                        
                        
                    }
                }
                var monsterResult = _matriOfMonsters.GetTile(i, j);
                if (monsterResult != null)
                {
                    if (monsterResult.MonsterType != MonsterType.Empty)
                    {
                        var position = _layout.CellToWorld(new Vector3Int(i + 1, -j + 1, 0));
                        var instance = Instantiate(monsterResult.Monster, position, Quaternion.identity);
                        
                    }
                }
            }
        }
    }

    private List<Dictionary<TileSO, float>> GetInitialFirstLayer()
    {
        List<Dictionary<TileSO, float>> initialTiles = new List<Dictionary<TileSO, float>>();
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (IsDoorOnPosition(i, j))
                    initialTiles.Add(DoorsTiles.ToDictionary(x => x, x=>1000f/(float)DoorsTiles.Count));
                else
                    initialTiles.Add(_tiles.ToDictionary(x => x, x => 1000f / (float)_tiles.Count));
            }
        }
        return initialTiles;
    }

    private List<Dictionary<ObstacleSO,float>> GetInitialSecondLayer()
    {

        List<Dictionary<ObstacleSO, float>> initialObstacles = new List<Dictionary<ObstacleSO, float>>();
        var emptyListObstacles = new List<KeyValuePair<ObstacleSO, float>>() { new KeyValuePair<ObstacleSO, float>(EmptyObstacle, 1.0f) };


        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (_matrixOfTiles.GetTile(i, j) != null && !IsDoorOnPosition(i, j))
                {
                    var dic = _matrixOfTiles.GetTile(i, j)?.secondLayerRestrictions;
                    initialObstacles.Add(dic.ToDictionary(x => _obstacles.First(y => y.obstacleType == x.obstacle), x => x.chance));
                }
                else
                {
                    initialObstacles.Add(new Dictionary<ObstacleSO, float>(emptyListObstacles));
                }


            }
        }
        return initialObstacles;
    }

    private List<Dictionary<MonsterSO, float>> GetInitialThirdLayer()
    {
        List<Dictionary<MonsterSO, float>> initialMonsters = new List<Dictionary<MonsterSO, float>>();
        var emptyListMonsters = new List<KeyValuePair<MonsterSO, float>>() { new KeyValuePair<MonsterSO, float>(EmptySpawn, 1.0f) };


        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (_matrixOfTiles.GetTile(i, j) != null && _matriOfObstacles.GetTile(i, j) != null && !IsDoorOnPosition(i,j))
                {
                    var dicFromTiles = _matrixOfTiles.GetTile(i, j)?.spawnableMonsters;
                    var dicFromObstacles = _matriOfObstacles.GetTile(i, j)?.spawnableMonsters;
                    var initialDic = dicFromTiles.ToDictionary(x => _monsters.First(y => y.MonsterType == x.monster), x => x.chance);
                    foreach (var pair in dicFromObstacles)
                    {
                        var monsterObject = _monsters.First(y => y.MonsterType == pair.monster);
                        if (initialDic.ContainsKey(monsterObject))
                        {
                            initialDic[monsterObject] = initialDic[monsterObject] * pair.chance;
                        }
                        else
                        {
                            initialDic.Add(monsterObject, pair.chance);
                        }
                    }
                    initialMonsters.Add(initialDic);

                }
                else
                {
                    initialMonsters.Add(new Dictionary<MonsterSO, float>(emptyListMonsters));
                }
            }
        }
        return initialMonsters;
    }

    private bool IsDoorOnPosition(int i, int j)
    {
        if(j == y / 2 - 1 || j == y / 2 || j == y / 2 + 1 || j == y / 2 + 2)
        {
            if ((i == 0 || i == 1) && isDoorOnSide[2]) return true;
            if ((i == x - 1|| i == x-2) && isDoorOnSide[0]) return true;
        }

        if (i == x / 2 - 1 || i == x / 2 || i == x / 2 + 1 || i == x / 2 + 2)
        {
            if ((j == 0 || j == 1) && isDoorOnSide[1]) return true;
            if ((j == y - 1 || j == x - 2) && isDoorOnSide[3]) return true;
        }

        return false;
    }
    private static void ResolveMatrix<T>(MapMatrix<T> matrix) where T : ITileKind<T>
    {
        while (matrix.AreAllTilesSet())
        {
            (int i, int j) tileCords = matrix.PickRandomTile();
            matrix.PickTileValue(tileCords.i, tileCords.j);
            matrix.RemoveImposiblePairs();
        }
    }

}
