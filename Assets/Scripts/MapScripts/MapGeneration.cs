using MapScripts.Obstacles;
using MapScripts.Tiles;
using MonsterScripts;
using System.Collections.Generic;
using System.Linq;
using UnderBounders;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapScripts
{
    /// <summary>
    /// Controlls room generation
    /// </summary>
    public class MapGeneration : MonoBehaviour
    {
        /// <summary>
        /// Room generation instance of singleton
        /// </summary>
        public static MapGeneration instance;

        /// <summary>
        /// Size of the room
        /// </summary>
        public int x, y;
        /// <summary>
        /// Ramdom generator seed for the room
        /// </summary>
        public int seed = 23;
        /// <summary>
        /// Determins if the room generation is done
        /// </summary>
        public bool isDone { get; set; }
        /// <summary>
        /// List of chests opened
        /// </summary>
        public List<(int x, int y)> isChestOpen { get; set; }
        /// <summary>
        /// Determines on which side the doors should be
        /// </summary>
        public bool[] IsDoorOnSide { get; set; }
        /// <summary>
        /// Floor tiles tilemap
        /// </summary>
        [SerializeField] private Tilemap tilesTilemap;
        /// <summary>
        /// Obstacles tilemap
        /// </summary>
        [SerializeField] private Tilemap obstaclesTilemap;
        /// <summary>
        /// Walls tilemap
        /// </summary>
        [SerializeField] private Tilemap _wallsTilemap;
        /// <summary>
        /// Grid layout to postition nontileable object in the game
        /// </summary>
        [SerializeField] private Grid _layout;
        /// <summary>
        /// Floor tiles available list
        /// </summary>
        [SerializeField] private List<TileSO> tiles;
        /// <summary>
        /// Obstacles available list
        /// </summary>
        [SerializeField] private List<ObstacleSO> obstacles;
        /// <summary>
        /// Monsters available list
        /// </summary>
        [SerializeField] private List<MonsterSO> monsters;
        /// <summary>
        /// Tiles that needs to spawn near the doors
        /// </summary>
        [SerializeField] private List<TileSO> doorsTiles;
        /// <summary>
        /// Empty obstacle to spawn near the doors
        /// </summary>
        [SerializeField] private ObstacleSO emptyObstacle;
        /// <summary>
        /// Empty spawn point for area near the doors
        /// </summary>
        [SerializeField] private MonsterSO emptySpawn;
        /// <summary>
        /// Borders tile for uppe wall
        /// </summary>
        [SerializeField] private List<Tile> borderTileUp;
        /// <summary>
        /// Walls tiles for other sides
        /// </summary>
        [SerializeField] private List<Tile> borderTileSide;
        /// <summary>
        /// Doors tiles
        /// </summary>
        [SerializeField] private List<GameObject> doors;

        /// <summary>
        /// Describes a chance of spawning each and every kind of floor tile
        /// </summary>
        [SerializeField] private List<KindOfFloorConstraint> floorChanceMod;

        /// <summary>
        /// Current object to destroy on map unload or reload
        /// </summary>
        private List<GameObject> _currObjects;
        /// <summary>
        /// Matrix of floor tiles
        /// </summary>
        private WFCMatrix<TileSO> _matrixOfTiles;
        /// <summary>
        /// Matrix of obstacles
        /// </summary>
        private WFCMatrix<ObstacleSO> _matrixOfObstacles;
        /// <summary>
        /// Matrix of monsters
        /// </summary>
        private WFCMatrix<MonsterSO> _matrixOfMonsters;



        private void Awake()
        {
            instance = this;
            isDone = false;
        }


        void Start()
        {
            _currObjects = new List<GameObject>();
        }

        /// <summary>
        /// Clears map from all of the tiles
        /// </summary>
        public void ClearMap()
        {
            foreach (var objectToDestroy in _currObjects)
                Destroy(objectToDestroy);
            tilesTilemap.ClearAllTiles();
            obstaclesTilemap.ClearAllTiles();
            _wallsTilemap.ClearAllTiles();
        }

        /// <summary>
        /// Main world generation function
        /// </summary>
        /// <param name="seed">
        /// Room random generation seed
        /// </param>
        /// <param name="x">
        /// Room number of collumns
        /// </param>
        /// <param name="y">
        /// Room number of rows
        /// </param>
        public void GenerateRoom(int seed, int x, int y)
        {
            this.x = x;
            this.y = y;
            Random.InitState(seed);
            GenerateOutSide();
            GenerateInside();
            isDone = true;

        }

        /// <summary>
        /// Generates walls and doors
        /// </summary>
        private void GenerateOutSide()
        {
            SetCorners();
            SetSideWalls();
            SetUpDownWalls();
        }

        /// <summary>
        /// Sets the corners of the walls in correct roation
        /// </summary>
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

        /// <summary>
        /// Sets sideway walls and doors
        /// </summary>
        private void SetSideWalls()
        {
            Vector3Int pos;
            for (int i = 2; i > -y; i--)
            {
                if ((i == -y / 2 || i == -y / 2 - 1) && IsDoorOnSide[0])
                {
                    pos = new Vector3Int(x + 1, i + 1, 0);
                    var instance = Instantiate(doors[0], _layout.CellToWorld(pos), Quaternion.Euler(0, 0, 0));
                    _currObjects.Add(instance);
                    instance.GetComponent<DoorsController>().dir = Direction.right;
                }
                else
                {
                    pos = new Vector3Int(x, i, 0);
                    _wallsTilemap.SetTile(pos, borderTileSide[0]);
                }
                if ((i == -y / 2 || i == -y / 2 - 1) && IsDoorOnSide[2])
                {
                    pos = new Vector3Int(0, i + 1, 0);
                    var instance = Instantiate(doors[0], _layout.CellToWorld(pos), Quaternion.Euler(0, 0, 180f));
                    _currObjects.Add(instance);
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

        /// <summary>
        /// Sets up upward and downwards walls and door
        /// </summary>
        private void SetUpDownWalls()
        {
            Vector3Int pos;
            for (int i = 0; i < x; i++)
            {
                if ((i == x / 2 || i == x / 2 + 1) && IsDoorOnSide[1])
                {

                    pos = new Vector3Int(i + 1, 2, 0);
                    var tempPos = _layout.CellToWorld(pos);
                    var instance = Instantiate(doors[1], new Vector3(tempPos.x, tempPos.y + 0.08f, tempPos.z), Quaternion.Euler(0, 0, 0));
                    _currObjects.Add(instance);
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
                if ((i == x / 2 || i == x / 2 + 1) && IsDoorOnSide[3])
                {
                    pos = new Vector3Int(i + 1, -y + 1, 0);
                    var instance = Instantiate(doors[0], _layout.CellToWorld(pos), Quaternion.Euler(0, 0, 270f));
                    _currObjects.Add(instance);
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

        /// <summary>
        /// Manages room generation inside borders
        /// </summary>
        private void GenerateInside()
        {

            var tilesInit = GetInitialFirstLayer();
            _matrixOfTiles = new WFCMatrix<TileSO>(x, y, tilesInit);

            _matrixOfTiles.ResolveMatrix();

            var initialObstacles = GetInitialSecondLayer();
            _matrixOfObstacles = new WFCMatrix<ObstacleSO>(x, y, initialObstacles);

            _matrixOfObstacles.ResolveMatrix();

            var initialMonsters = GetInitialThirdLayer();

            _matrixOfMonsters = new WFCMatrix<MonsterSO>(x, y, initialMonsters);

            _matrixOfMonsters.ResolveMatrix();

            PlaceTiles();
        }
        /// <summary>
        /// Places tiles in correct places in the grid
        /// </summary>
        private void PlaceTiles()
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    var result = _matrixOfTiles.GetTile(i, j);
                    if (result != null)
                    {
                        Tile tile = result.tile;
                        tilesTilemap.SetTile(new Vector3Int(i, -j, 0), tile);
                    }
                    var obstacleResult = _matrixOfObstacles.GetTile(i, j);
                    if (obstacleResult != null)
                    {
                        if (obstacleResult.tile is Tile obstacleTile)
                            obstaclesTilemap.SetTile(new Vector3Int(i, -j, 0), obstacleTile);
                        if (obstacleResult.tile is GameObject obstacleObject && obstacleResult.obstacleType != ObstacleType.Empty)
                        {
                            var position = _layout.CellToWorld(new Vector3Int(i + 1, -j + 1, 0));
                            var instance = Instantiate(obstacleObject, position, Quaternion.identity);
                            _currObjects.Add(instance);

                            var control = instance.GetComponent<ChestController>();
                            if (control != null)
                            {
                                control.chestPos = (i, j);
                                if (isChestOpen != null && isChestOpen.Any() && isChestOpen.Any(t => t.x == i && t.y == j))
                                {
                                    control.ChangeSprite();
                                }
                            }


                        }
                    }
                    var monsterResult = _matrixOfMonsters.GetTile(i, j);
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

        /// <summary>
        /// Creates initial floor tile matrix
        /// </summary>
        /// <returns>
        /// Initial state tile possiblities list for the floor tiles matrix
        /// </returns>
        private List<Dictionary<TileSO, float>> GetInitialFirstLayer()
        {
            List<Dictionary<TileSO, float>> initialTiles = new List<Dictionary<TileSO, float>>();
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (IsDoorOnPosition(i, j))
                        initialTiles.Add(doorsTiles.ToDictionary(x => x, x => (1000f / (float)doorsTiles.Count) * floorChanceMod.First(y => x.KindOfTile == y.tileKind).chanceMod));
                    else
                        initialTiles.Add(tiles.ToDictionary(x => x, x => (1000f / (float)doorsTiles.Count) * floorChanceMod.First(y => x.KindOfTile == y.tileKind).chanceMod));
                }
            }
            return initialTiles;
        }

        /// <summary>
        /// Creates initial obstacles matrix
        /// </summary>
        /// <returns>
        /// Initial state tile possiblities list for the obstacles matrix
        /// </returns>
        private List<Dictionary<ObstacleSO, float>> GetInitialSecondLayer()
        {

            List<Dictionary<ObstacleSO, float>> initialObstacles = new List<Dictionary<ObstacleSO, float>>();
            var emptyListObstacles = new List<KeyValuePair<ObstacleSO, float>>() { new KeyValuePair<ObstacleSO, float>(emptyObstacle, 1.0f) };


            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (_matrixOfTiles.GetTile(i, j) != null && !IsDoorOnPosition(i, j))
                    {
                        var tileDic = _matrixOfTiles.GetTile(i, j)?.secondLayerRestrictions;
                        var initialDic = obstacles.ToDictionary(x => x, x => 1000f / (float)obstacles.Count);
                        var keys = new List<ObstacleSO>(initialDic.Keys);
                        foreach (var obstacle in keys)
                        {
                            float chanceFromTile = tileDic.FirstOrDefault(x => obstacle.obstacleType == x.obstacle)?.chance ?? 1;
                            initialDic[obstacle] *= chanceFromTile;
                        }

                        initialObstacles.Add(initialDic);
                    }
                    else
                    {
                        initialObstacles.Add(new Dictionary<ObstacleSO, float>(emptyListObstacles));
                    }


                }
            }
            return initialObstacles;
        }

        /// <summary>
        /// Creates initial monsters matrix
        /// </summary>
        /// <returns>
        /// Initial state tile possiblities list for the monsters matrix
        /// </returns>
        private List<Dictionary<MonsterSO, float>> GetInitialThirdLayer()
        {
            List<Dictionary<MonsterSO, float>> initialMonsters = new List<Dictionary<MonsterSO, float>>();
            var emptyListMonsters = new List<KeyValuePair<MonsterSO, float>>() { new KeyValuePair<MonsterSO, float>(emptySpawn, 1.0f) };


            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (_matrixOfTiles.GetTile(i, j) != null && _matrixOfObstacles.GetTile(i, j) != null && !IsDoorOnPosition(i, j))
                    {
                        var dicFromTiles = _matrixOfTiles.GetTile(i, j)?.spawnableMonsters;
                        var dicFromObstacles = _matrixOfObstacles.GetTile(i, j)?.spawnableMonsters;
                        var initialDic = monsters.ToDictionary(x => x, x => 1000f / (float)monsters.Count);
                        var keys = new List<MonsterSO>(initialDic.Keys);
                        foreach (var monster in keys)
                        {
                            float chanceFromTile = dicFromTiles.FirstOrDefault(x => monster.MonsterType == x.monster)?.chance ?? 1;
                            float chanceFromObstacle = dicFromObstacles.FirstOrDefault(x => monster.MonsterType == x.monster)?.chance ?? 1;
                            initialDic[monster] *= chanceFromTile * chanceFromObstacle;
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

        /// <summary>
        /// Check if the doors should be on the postition
        /// </summary>
        /// <param name="i">
        /// Collumn in the grid
        /// </param>
        /// <param name="j">
        /// Row in the grid
        /// </param>
        /// <returns>
        /// <see langword="true"/> if doors should be added, <see langword="false"/> otherwise
        /// </returns>
        private bool IsDoorOnPosition(int i, int j)
        {
            if (j == y / 2 - 1 || j == y / 2 || j == y / 2 + 1 || j == y / 2 + 2)
            {
                if ((i == 0 || i == 1) && IsDoorOnSide[2]) return true;
                if ((i == x - 1 || i == x - 2) && IsDoorOnSide[0]) return true;
            }

            if (i == x / 2 - 1 || i == x / 2 || i == x / 2 + 1 || i == x / 2 + 2)
            {
                if ((j == 0 || j == 1) && IsDoorOnSide[1]) return true;
                if ((j == y - 1 || j == y - 2) && IsDoorOnSide[3]) return true;
            }

            return false;
        }
    }
}
