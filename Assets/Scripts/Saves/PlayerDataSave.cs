using System;

/// <summary>
/// Class responsible of containing current player data
/// </summary>
[Serializable]
public class PlayerDataSave
{
    /// <summary>
    /// Current player pos on x axis in the room
    /// </summary>
    public int PlayerPosx { get; set; }
    /// <summary>
    /// Current player pos on y axis in the room
    /// </summary>
    public int PlayerPosy { get; set; }
    /// <summary>
    /// Currnet player health
    /// </summary>
    public float Health { get; set; }
    /// <summary>
    /// Current ammo state of player
    /// </summary>
    public int SecondaryAmmo { get; set; }
}
