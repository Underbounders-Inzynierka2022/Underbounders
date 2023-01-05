using System.Collections.Generic;

/// <summary>
/// Class containing all rooms data from particular room in the dungeon
/// </summary>
public class Room
{
    public int x { get; set; }
    public int y { get; set; }
    public int Seed { get; set; }

    public List<(int x, int y)> ChestOpened { get; set; }

    public RoomSO RoomKind { get; set; }
    public bool IsConquered { get; set; }

    public Room(int i, int j, RoomSO roomKind, int seed)
    {
        x = i;
        y = j;
        this.RoomKind = roomKind;
        this.Seed = seed;
        IsConquered = false;
        ChestOpened = new List<(int, int)>();
    }

    public bool[] GetRoomsDoorsAsArray()
    {
        var arr = new bool[4];
        arr[0] = RoomKind.IsDoorRight;
        arr[1] = RoomKind.IsDoorUp;
        arr[2] = RoomKind.IsDoorLeft;
        arr[3] = RoomKind.IsDoorDown;
        return arr;
    }
}
