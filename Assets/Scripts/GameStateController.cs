using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

/// <summary>
/// Class managing state of the game, responsible for pausing and etc.
/// </summary>
public class GameStateController : MonoBehaviour
{
    /// <summary>
    /// Game state controlle instance
    /// </summary>
    public static GameStateController instance { get; set; }
    /// <summary>
    /// Determines if the game is paused
    /// </summary>
    public bool isPaused { get; set; }
    /// <summary>
    /// Determines if the game is in process of swithing the rooms
    /// </summary>
    public bool isSwitchingRoom { get; set; }
    /// <summary>
    /// Rooms matrix
    /// </summary>
    public Room[][] rooms { get; set; }
    /// <summary>
    /// Current room
    /// </summary>
    public Room currentRoom { get; set; }

    /// <summary>
    /// Available room kinds
    /// </summary>
    [SerializeField] private List<RoomSO> roomKinds;
    /// <summary>
    /// Loading screen object
    /// </summary>
    [SerializeField] private GameObject loadScreen;
    /// <summary>
    /// Main menu object
    /// </summary>
    [SerializeField] private GameObject mainMenu;
    /// <summary>
    /// Pause Menu object
    /// </summary>
    [SerializeField] private GameObject pauseMenu;
    /// <summary>
    /// Status bars with health and ammo
    /// </summary>
    [SerializeField] private UIDocument statusBars;
    /// <summary>
    /// Game over screen object
    /// </summary>
    [SerializeField] private GameObject gameOver;
    /// <summary>
    /// Game won screen game object
    /// </summary>
    [SerializeField] private GameObject gameWon;
    /// <summary>
    /// Defines how big the dungeon is supposed to be as a lenght of square matrix border
    /// </summary>
    [SerializeField] private int dungeonWallSize = 5;

    /// <summary>
    /// World seed
    /// </summary>
    private int seed = 757662750;
    /// <summary>
    /// Current room
    /// </summary>
    private (int x, int y) currentRoomPos;
    /// <summary>
    /// Player possition
    /// </summary>
    private (int x, int y) playerPos;
    /// <summary>
    /// Current timer value
    /// </summary>
    private float timer = 0f;
    /// <summary>
    /// Initial matrix for the rooms
    /// </summary>
    private List<Dictionary<RoomSO, float>> _initialMatrix;




    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitializeMatrix();
        statusBars.rootVisualElement.style.visibility = Visibility.Hidden;
        mainMenu.SetActive(true);
        isPaused = false;
        isSwitchingRoom = false;
    }

    void Update()
    {
        if (isPaused)
            return;
        timer += Time.deltaTime;
    }

    /// <summary>
    /// Initialize new game coroutine, on click
    /// </summary>
    public void NewGame()
    {
        StartCoroutine(NewGameGeneration());
    }

    /// <summary>
    /// Loads main menu
    /// </summary>
    public void LoadMainMenu()
    {
        MapClear();
        InitializeMatrix();
        pauseMenu.SetActive(false);
        gameOver.SetActive(false);
        gameWon.SetActive(false);
        statusBars.rootVisualElement.style.visibility = Visibility.Hidden;
        mainMenu.SetActive(true);
    }

    /// <summary>
    /// Exits rooms and enters new one in given direction
    /// </summary>
    /// <param name="dir">
    /// Direction that player go towards
    /// </param>
    public void SwitchRooms(Direction dir)
    {
        isPaused = true;
        if (isSwitchingRoom) return;
        isSwitchingRoom = true;
        currentRoom.IsConquered = true;
        GameWinningCheck();
        switch (dir)
        {
            case Direction.up:
                currentRoomPos.y -= 1;
                break;
            case Direction.right:
                currentRoomPos.x += 1;
                break;
            case Direction.left:
                currentRoomPos.x -= 1;
                break;
            case Direction.down:
                currentRoomPos.y += 1;
                break;
        }
        StartCoroutine(GenerateNextRoom(dir));
    }


    /// <summary>
    /// Loads pause menu
    /// </summary>
    public void LoadPauseMenu()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// UnloadsPauseMenu
    /// </summary>
    public void UnloadPauseMenu()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Ends game on death of player
    /// </summary>
    public void OnGameEnd()
    {
        isPaused = true;
        gameOver.SetActive(true);
    }

    /// <summary>
    /// Saves game on click
    /// </summary>
    public void SaveGame()
    {
        SaveData.Current.Player = new PlayerDataSave();
        SaveData.Current.Player.PlayerPosx = playerPos.x;
        SaveData.Current.Player.PlayerPosy = playerPos.y;
        SaveData.Current.Player.Health = PlayerStatsController.Instance.GetHealth();
        SaveData.Current.Player.SecondaryAmmo = PlayerStatsController.Instance.GetAmmo();
        SaveData.Current.Rooms = new List<RoomsDataSave>();
        for (int i = 0; i < dungeonWallSize; i++)
        {
            for (int j = 0; j < dungeonWallSize; j++)
            {
                RoomsDataSave room = new RoomsDataSave();
                room.ChestOpened = rooms[i][j].ChestOpened;
                room.IsConquered = rooms[i][j].IsConquered;
                room.RoomPosx = i;
                room.RoomPosy = j;
                SaveData.Current.Rooms.Add(room);
            }
        }
        SaveData.Current.Timer = timer;
        SaveData.Current.CurrentRoom = (currentRoomPos.x, currentRoomPos.y);
        SaveData.Current.Seed = seed;
        SerializationManager.Save("current", SaveData.Current);
    }

    /// <summary>
    /// Loads game on click
    /// </summary>
    public void LoadGame()
    {
        var loaded = SerializationManager.Load($"{Application.persistentDataPath}/saves/current.save");
        if (loaded is null)
            return;
        SaveData.Current = (SaveData)loaded;
        seed = SaveData.Current.Seed;
        StartCoroutine(LoadGameGeneration());
    }

    /// <summary>
    /// Exits game on click
    /// </summary>
    public void Exit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    /// <summary>
    /// Generates dungeon populating it with rooms
    /// </summary>
    private void GenerateWorld()
    {
        Random.InitState(seed);
        var roomsMap = new WFCMatrix<RoomSO>(dungeonWallSize, dungeonWallSize, _initialMatrix);
        roomsMap.ResolveMatrix();
        rooms = new Room[dungeonWallSize][];
        for (int i = 0; i < dungeonWallSize; i++)
        {
            rooms[i] = new Room[dungeonWallSize];
            for (int j = 0; j < dungeonWallSize; j++)
            {
                int localSeed = Random.Range(int.MinValue, int.MaxValue);
                int x = Random.Range(10,31);
                int y = Random.Range(10, 31);
                rooms[i][j] = new Room(x, y,roomsMap.GetTile(i,j),localSeed);
            }
        }
    }

    /// <summary>
    /// New game creation Coroutine
    /// </summary>
    /// <returns>
    /// Coroutines enumerator
    /// </returns>
    private IEnumerator NewGameGeneration()
    {
        LoadLoadingScreen();
        yield return new WaitForFixedUpdate();
        seed = Random.Range(int.MinValue, int.MaxValue);
        GenerateWorld();
        currentRoom = rooms[0][0];
        currentRoomPos.x = 0;
        currentRoomPos.y = 0;
        playerPos.x = currentRoom.x / 2;
        playerPos.y = currentRoom.y - 1;
        PlayerStatsController.Instance.SetPlayerCords(currentRoom.x / 2, currentRoom.y-1);
        PlayerStatsController.Instance.SetPlayerBegginningStats();
        GenerateRoom();
        HideLoadingScreen();
        yield return new WaitForSeconds(0.2f);
        isPaused = false;
        timer = 0;
    }



    /// <summary>
    /// Recreates game state from save file data coroutine
    /// </summary>
    /// <returns>
    /// Coroutines enumerator
    /// </returns>
    private IEnumerator LoadGameGeneration()
    {
        LoadLoadingScreen();
        yield return new WaitForFixedUpdate();
        GenerateWorld();
        for(int i = 0; i< dungeonWallSize; i++)
        {
            for(int j = 0; j < dungeonWallSize; j++)
            {
                rooms[i][j].ChestOpened = SaveData.Current.Rooms.First(x => x.RoomPosx == i && x.RoomPosy == j).ChestOpened;
                rooms[i][j].IsConquered = SaveData.Current.Rooms.First(x => x.RoomPosx == i && x.RoomPosy == j).IsConquered;
            }
        }
        currentRoom = rooms[SaveData.Current.CurrentRoom.x][SaveData.Current.CurrentRoom.y];
        currentRoomPos.x = SaveData.Current.CurrentRoom.x;
        currentRoomPos.y = SaveData.Current.CurrentRoom.y;
        playerPos.x = SaveData.Current.Player.PlayerPosx;
        playerPos.y = SaveData.Current.Player.PlayerPosy;
        PlayerStatsController.Instance.SetPlayerCords(playerPos.x, playerPos.y);
        PlayerStatsController.Instance.SetPlayerLoadedStats(SaveData.Current.Player.Health, SaveData.Current.Player.SecondaryAmmo);
        GenerateRoom();
        HideLoadingScreen();
        yield return new WaitForSeconds(0.2f);
        isPaused = false;
        timer = SaveData.Current.Timer;
    }

    /// <summary>
    /// Generates next room and unload prevous in coroutine
    /// </summary>
    /// <param name="dir">
    /// Direction to teleport player to
    /// </param>
    /// <returns>
    /// Coroutines enumerator
    /// </returns>
    private IEnumerator GenerateNextRoom(Direction dir)
    {
        LoadLoadingScreen();
        MapClear();
        yield return new WaitForFixedUpdate();
        currentRoom = rooms[currentRoomPos.x][currentRoomPos.y];
        switch (dir)
        {
            case Direction.up:
                playerPos.x = currentRoom.x / 2;
                playerPos.y = currentRoom.y - 1;
                PlayerStatsController.Instance.SetPlayerCords(currentRoom.x / 2, currentRoom.y - 1);
                break;
            case Direction.right:
                playerPos.x = 0;
                playerPos.y = currentRoom.y / 2 + 1;
                PlayerStatsController.Instance.SetPlayerCords(0, currentRoom.y / 2 + 1);
                break;
            case Direction.left:
                playerPos.x = currentRoom.x - 1;
                playerPos.y = currentRoom.y / 2 + 1;
                PlayerStatsController.Instance.SetPlayerCords(currentRoom.x - 1, currentRoom.y / 2 + 1);
                break;
            case Direction.down:
                playerPos.x = currentRoom.x / 2;
                playerPos.y = 1;
                PlayerStatsController.Instance.SetPlayerCords(currentRoom.x / 2, 1);
                break;
        }
        GenerateRoom();
        HideLoadingScreen();
        yield return new WaitForSeconds(0.2f);
        isPaused = false;
        isSwitchingRoom = false;
    }

    /// <summary>
    /// Initialize rooms array, befor world generation
    /// </summary>
    private void InitializeMatrix()
    {
        _initialMatrix = new List<Dictionary<RoomSO, float>>();
        for (int i = 0; i < dungeonWallSize; i++)
        {
            for (int j = 0; j < dungeonWallSize; j++)
            {
                if (j == 0 && i == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorLeft || x.IsDoorUp)&& x.IsDoorRight && x.IsDoorDown).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorLeft || x.IsDoorUp) && x.IsDoorRight && x.IsDoorDown)));
                else if (j == dungeonWallSize - 1 && i == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorLeft || x.IsDoorDown) && x.IsDoorRight && x.IsDoorUp).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorLeft || x.IsDoorDown) && x.IsDoorRight && x.IsDoorUp)));
                else if (j == dungeonWallSize - 1 && i == dungeonWallSize - 1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorRight || x.IsDoorDown) && x.IsDoorLeft && x.IsDoorUp).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorRight || x.IsDoorDown) && x.IsDoorLeft && x.IsDoorUp)));
                else if (j == 0 && i == dungeonWallSize - 1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorRight || x.IsDoorUp) && x.IsDoorLeft && x.IsDoorDown).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorRight || x.IsDoorUp) && x.IsDoorLeft && x.IsDoorDown)));
                else if (j == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorUp)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorUp))));
                else if (i == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorLeft)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorLeft))));
                else if (j == dungeonWallSize - 1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorDown)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorDown))));
                else if (i == dungeonWallSize - 1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorRight)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorRight))));
                else if(dungeonWallSize>3 && i == dungeonWallSize/2 && j == dungeonWallSize/2)
                    _initialMatrix.Add(roomKinds.Where(x => x.IsDoorRight && x.IsDoorUp && x.IsDoorLeft && x.IsDoorDown).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => x.IsDoorRight && x.IsDoorUp && x.IsDoorLeft && x.IsDoorDown)));
                else _initialMatrix.Add(roomKinds.ToDictionary(x => x, x => 100f / (float)roomKinds.Count));
            }
        }
    }

    /// <summary>
    /// Hide menus and sets up loading screen
    /// </summary>
    private void LoadLoadingScreen()
    {
        loadScreen.SetActive(true);
        statusBars.rootVisualElement.style.visibility = Visibility.Hidden;
        mainMenu.SetActive(false);
        isPaused = true;
    }
    /// <summary>
    /// Hides loading screen
    /// </summary>
    private void HideLoadingScreen()
    {
        loadScreen.SetActive(false);
        statusBars.rootVisualElement.style.visibility = Visibility.Visible;
    }
    /// <summary>
    /// Calls room generation for current room
    /// </summary>
    private void GenerateRoom()
    {
        if (currentRoom is null) return;
        if (currentRoom.ChestOpened != null) MapGeneration.instance.isChestOpen = currentRoom.ChestOpened;
        MapGeneration.instance.IsDoorOnSide = currentRoom.GetRoomsDoorsAsArray();
        MapGeneration.instance.GenerateRoom(currentRoom.Seed, currentRoom.x, currentRoom.y);
    }

    /// <summary>
    /// Finishes game if winning condition is satisfied
    /// </summary>
    private void GameWinningCheck()
    {
        if (!CheckConquere()) return;

        gameWon.SetActive(true);
        var text = GameObject.FindGameObjectWithTag("Time");
        int minutes = (int)(timer / 60);
        text.GetComponent<TextMeshProUGUI>().SetText($"{minutes.ToString("D3")}:{((int)(timer - minutes * 60)).ToString("D2")}");
        statusBars.rootVisualElement.style.visibility = Visibility.Hidden;
        MapClear();

    }

    /// <summary>
    /// Check if all of rooms were discovered
    /// </summary>
    /// <returns>
    /// True if all of the available rooms were conqeured, false if any of the rooms weren't discovered
    /// </returns>
    private bool CheckConquere()
    {
        for (int i = 0; i<dungeonWallSize;i++)
        {
            for (int j = 0; j < dungeonWallSize; j++)
            {
                if (!rooms[i][j].IsConquered && rooms[i][j].RoomKind != null)
                    return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Clears map from any unnecessary object
    /// </summary>
    public void MapClear()
    {
        var items = GameObject.FindGameObjectsWithTag("Item");
        foreach (var item in items) Destroy(item);
        items = GameObject.FindGameObjectsWithTag("Turret");
        foreach (var item in items) Destroy(item);
        items = GameObject.FindGameObjectsWithTag("MeleeEnemy");
        foreach (var item in items) Destroy(item);
        items = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var item in items) Destroy(item);
        MapGeneration.instance.ClearMap();
    }
}

