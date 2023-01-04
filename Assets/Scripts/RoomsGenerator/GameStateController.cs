using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class GameStateController : MonoBehaviour
{
    public static GameStateController instance;
    public Room[][] rooms;
    public bool isPaused = false;
    public bool isSwitching = false;
    [SerializeField] private List<RoomSO> roomKinds;
    [SerializeField] private GameObject LoadScreen;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private UIDocument bars;
    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject GameWon;

    public Room currentRoom;
    private (int x, int y) currentRoomPos;
    private (int x, int y) playerPos;
    private int seed = -740281645;
    [SerializeField] private int dungeonWallSize = 5;
    

    float timer = 0f;

    private List<Dictionary<RoomSO, float>> _initialMatrix;

    private bool isRoomArrayDone = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializeMatrix();
        bars.rootVisualElement.style.visibility = Visibility.Hidden;
        MainMenu.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
            return;
        timer += Time.deltaTime;
    }

    private void InitializeMatrix()
    {
        _initialMatrix = new List<Dictionary<RoomSO, float>>();
        for (int i = 0; i < dungeonWallSize; i++)
        {
            for (int j = 0; j < dungeonWallSize; j++)
            {
                if (j == 0 && i == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorLeft || x.IsDoorUp)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorLeft || x.IsDoorUp))));
                else if (j == dungeonWallSize - 1 && i == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorLeft || x.IsDoorDown)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorLeft || x.IsDoorDown))));
                else if (j == dungeonWallSize - 1 && i == dungeonWallSize-1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorRight || x.IsDoorDown)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorRight || x.IsDoorDown))));
                else if (j == 0 && i == dungeonWallSize - 1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorRight || x.IsDoorUp)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorRight || x.IsDoorUp))));
                else if (j == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorUp)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorUp))));
                else if (i == 0)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorLeft)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorLeft))));
                else if (j == dungeonWallSize - 1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorDown)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorDown))));
                else if (i == dungeonWallSize - 1)
                    _initialMatrix.Add(roomKinds.Where(x => !(x.IsDoorRight)).ToDictionary(x => x, x => 100f / (float)roomKinds.Count(x => !(x.IsDoorRight))));
                else _initialMatrix.Add(roomKinds.ToDictionary(x => x, x => 100f / (float)roomKinds.Count));
            }
        }
    }

    public void GenerateWorld()
    {
        Random.InitState(seed);
        var roomsMap = new MapMatrix<RoomSO>(dungeonWallSize, dungeonWallSize, _initialMatrix);
        while (roomsMap.AreAllTilesSet())
        {
            (int i, int j) tileCords = roomsMap.PickRandomTile();
            roomsMap.PickTileValue(tileCords.i, tileCords.j);
            roomsMap.RemoveImposiblePairs();

        }
        rooms = new Room[dungeonWallSize][];
        for (int i = 0; i < dungeonWallSize; i++)
        {
            rooms[i] = new Room[dungeonWallSize];
            for (int j = 0; j < dungeonWallSize; j++)
            {
                int localSeed = Random.Range(int.MinValue, int.MaxValue);
                int x = Random.Range(10,41);
                int y = Random.Range(10, 41);
                rooms[i][j] = new Room(x, y,roomsMap.GetTile(i,j),localSeed);
            }
        }
        isRoomArrayDone = true;
    }

    public void NewGame()
    {
        StartCoroutine(NewGameGeneration());

    }

    private IEnumerator NewGameGeneration()
    {
        LoadScreen.SetActive(true);
        bars.rootVisualElement.style.visibility = Visibility.Hidden;
        MainMenu.SetActive(false);
        yield return new WaitForFixedUpdate();
        seed = Random.Range(int.MinValue, int.MaxValue);
        GenerateWorld();
        currentRoom = rooms[0][0];
        currentRoomPos.x = 0;
        currentRoomPos.y = 0;
        isPaused = true;
        MapGeneration.instance.isDoorOnSide = currentRoom.GetRoomsDoorsAsArray();
        MapGeneration.instance.GenerateRoom(currentRoom.seed, currentRoom.x, currentRoom.y);
        playerPos.x = currentRoom.x / 2;
        playerPos.y = currentRoom.y - 1;
        PlayerStatsController.instance.SetPlayerCords(currentRoom.x / 2, currentRoom.y-1);
        PlayerStatsController.instance.SetPlayerBegginningStats();
        LoadScreen.SetActive(false);
        bars.rootVisualElement.style.visibility = Visibility.Visible;
        yield return new WaitForSeconds(0.2f);
        isPaused = false;
        timer = 0;
    }

    public IEnumerator LoadGameGeneration()
    {
        LoadScreen.SetActive(true);
        bars.rootVisualElement.style.visibility = Visibility.Hidden;
        MainMenu.SetActive(false);
        yield return new WaitForFixedUpdate();
        GenerateWorld();
        for(int i = 0; i< dungeonWallSize; i++)
        {
            for(int j = 0; j < dungeonWallSize; j++)
            {
                rooms[i][j].chestOpened = SaveData.current.rooms.First(x => x.roomPosx == i && x.roomPosy == j).chestOpened;
                rooms[i][j].IsConquered = SaveData.current.rooms.First(x => x.roomPosx == i && x.roomPosy == j).isConquered;
            }
        }
        currentRoom = rooms[SaveData.current.CurrentRoom.x][SaveData.current.CurrentRoom.y];
        currentRoomPos.x = SaveData.current.CurrentRoom.x;
        currentRoomPos.y = SaveData.current.CurrentRoom.y;
        isPaused = true;
        MapGeneration.instance.isDoorOnSide = currentRoom.GetRoomsDoorsAsArray();
        MapGeneration.instance.isChestOpen = currentRoom.chestOpened;
        MapGeneration.instance.GenerateRoom(currentRoom.seed, currentRoom.x, currentRoom.y);
        playerPos.x = SaveData.current.player.PlayerPosx;
        playerPos.y = SaveData.current.player.PlayerPosy;
        PlayerStatsController.instance.SetPlayerCords(playerPos.x, playerPos.y);
        PlayerStatsController.instance.SetPlayerLoadedStats(SaveData.current.player.Health, SaveData.current.player.SecondaryAmmo);
        LoadScreen.SetActive(false);
        bars.rootVisualElement.style.visibility = Visibility.Visible;
        yield return new WaitForSeconds(0.2f);
        isPaused = false;
        timer = 0;
    }

    public void LoadMainMenu()
    {
        MapClear();
        InitializeMatrix();
        PauseMenu.SetActive(false);
        GameOver.SetActive(false);
        GameWon.SetActive(false);
        bars.rootVisualElement.style.visibility = Visibility.Hidden;
        MainMenu.SetActive(true);
    }

    public void SwitchRooms(Direction dir)
    {
        if (isSwitching) return;
        isSwitching = true;
        currentRoom.IsConquered = true;
        if (CheckConquere()){
            GameWon.SetActive(true);
            var text = GameObject.FindGameObjectWithTag("Time");
            int minutes = (int)(timer / 60);
            text.GetComponent<TextMeshProUGUI>().SetText($"{minutes.ToString("D3")}:{((int)(timer - minutes*60)).ToString("D2")}");
            bars.rootVisualElement.style.visibility = Visibility.Hidden;
            MapClear();
            return;
        }
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

    private bool CheckConquere()
    {
        for (int i = 0; i<dungeonWallSize;i++)
        {
            for (int j = 0; j < dungeonWallSize; j++)
            {
                if (!rooms[i][j].IsConquered && rooms[i][j].roomKind != null)
                    return false;
            }
        }
        return true;
    }

    public IEnumerator GenerateNextRoom(Direction dir)
    {
        LoadScreen.SetActive(true);
        isPaused = true;
        yield return new WaitForFixedUpdate();
        currentRoom = rooms[currentRoomPos.x][currentRoomPos.y];
        
        MapClear();
        MapGeneration.instance.isChestOpen = currentRoom.chestOpened;
        MapGeneration.instance.isDoorOnSide = currentRoom.GetRoomsDoorsAsArray();
        MapGeneration.instance.GenerateRoom(currentRoom.seed, currentRoom.x, currentRoom.y);
        switch (dir)
        {
            case Direction.up:
                playerPos.x = currentRoom.x / 2;
                playerPos.y = currentRoom.y - 1;
                PlayerStatsController.instance.SetPlayerCords(currentRoom.x / 2, currentRoom.y - 1);
                break;
            case Direction.right:
                playerPos.x = 0;
                playerPos.y = currentRoom.y/2 + 1;
                PlayerStatsController.instance.SetPlayerCords(0, currentRoom.y / 2 + 1);
                break;
            case Direction.left:
                playerPos.x = currentRoom.x - 1;
                playerPos.y = currentRoom.y / 2 + 1;
                PlayerStatsController.instance.SetPlayerCords(currentRoom.x - 1, currentRoom.y / 2 + 1);
                break;
            case Direction.down:
                playerPos.x = currentRoom.x /2;
                playerPos.y = 1;
                PlayerStatsController.instance.SetPlayerCords(currentRoom.x / 2, 1);
                break;
        }
        
        LoadScreen.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        isPaused = false;
        timer = 0;
        isSwitching = false;
    }


    public void Exit()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

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

    public void LoadPauseMenu()
    {
        isPaused = true;
        PauseMenu.SetActive(true);
    }

    public void UnloadPauseMenu()
    {
        isPaused = false;
        PauseMenu.SetActive(false);
    }

    public void OnGameEnd()
    {
        isPaused = true;
        GameOver.SetActive(true);
    }

    public void SaveGame()
    {
        SaveData.current.player = new PlayerDataSave();
        SaveData.current.player.PlayerPosx = playerPos.x;
        SaveData.current.player.PlayerPosy = playerPos.y;
        SaveData.current.player.Health = PlayerStatsController.instance.GetHealth();
        SaveData.current.player.SecondaryAmmo = PlayerStatsController.instance.GetAmmo();
        SaveData.current.rooms = new List<RoomsDataSave>();
        for (int i = 0; i < dungeonWallSize; i++)
        {
            for (int j = 0; j < dungeonWallSize;j++)
            {
                RoomsDataSave room = new RoomsDataSave();
                room.chestOpened = rooms[i][j].chestOpened;
                room.isConquered = rooms[i][j].IsConquered;
                room.roomPosx = i;
                room.roomPosy = j;
                SaveData.current.rooms.Add(room);
            }
        }
        SaveData.current.timer = timer;
        SaveData.current.CurrentRoom = (currentRoomPos.x, currentRoomPos.y);
        SaveData.current.Seed = seed; 
        SerializationManager.Save("current", SaveData.current);
    }

    public void LoadGame()
    {
       var loaded = SerializationManager.Load($"{Application.persistentDataPath}/saves/current.save");
        if (loaded is null)
            return;
        SaveData.current = (SaveData)loaded;
        seed = SaveData.current.Seed;
        StartCoroutine(LoadGameGeneration());
    }
}

