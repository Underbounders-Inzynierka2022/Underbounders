using System;

/// <summary>
/// Class responsible of containing current player data
/// </summary>
[Serializable]
public class PlayerDataSave
{
    public int PlayerPosx { get; set; }
    public int PlayerPosy { get; set; }

    public float Health { get; set; }
    public int SecondaryAmmo { get; set; }
}
