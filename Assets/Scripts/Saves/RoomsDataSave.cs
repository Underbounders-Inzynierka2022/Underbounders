using System;
using System.Collections.Generic;

/// <summary>
/// Class responsible of containing current dungeon data
/// </summary>
[Serializable]
public class RoomsDataSave
{
    public int RoomPosx { get; set; }
    public int RoomPosy { get; set; }

    public List<(int x, int y)> ChestOpened { get; set; }
    public bool IsConquered { get; set; }
}
