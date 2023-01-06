using System;
using System.Collections.Generic;

/// <summary>
/// Class responsible of containing current dungeon data
/// </summary>
[Serializable]
public class RoomsDataSave
{
    /// <summary>
    /// Room collumn in the matrix
    /// </summary>
    public int RoomPosx { get; set; }
    /// <summary>
    /// Room row in the matrix
    /// </summary>
    public int RoomPosy { get; set; }
    /// <summary>
    /// List of the opened chests in the room
    /// </summary>
    public List<(int x, int y)> ChestOpened { get; set; }
    /// <summary>
    /// Determines if room was conquered
    /// </summary>
    public bool IsConquered { get; set; }
}
