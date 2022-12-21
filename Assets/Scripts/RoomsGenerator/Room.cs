using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public int x, y;
    public int seed;

    public List<(int x, int y)> chestOpened;

    public RoomSO roomKind;
    public bool IsConquered { get; set; }

    public Room(int i, int j, RoomSO roomKind, int seed)
    {
        x = i;
        y = j;
        this.roomKind = roomKind;
        this.seed = seed;
        IsConquered = false;
        chestOpened = new List<(int, int)>();
    }

    public bool[] GetRoomsDoorsAsArray()
    {
        var arr = new bool[4];
        arr[0] = roomKind.IsDoorRight;
        arr[1] = roomKind.IsDoorUp;
        arr[2] = roomKind.IsDoorLeft;
        arr[3] = roomKind.IsDoorDown;
        return arr;
    }
}
