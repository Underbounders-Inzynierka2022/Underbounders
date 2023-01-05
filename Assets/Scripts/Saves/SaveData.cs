using System;
using System.Collections.Generic;

/// <summary>
/// Class holding save data of the game
/// </summary>
[Serializable]
public class SaveData 
{
    private static SaveData s_current;
    public static SaveData Current
    {
        get
        {
            if(s_current is null)
            {
                s_current = new SaveData();
            }
            return s_current;
        }

        set {
            s_current = value;
        }
    }

    public PlayerDataSave Player { get; set; }
    public int Seed { get; set; }
    public List<RoomsDataSave> Rooms { get; set; }
    public (int x, int y) CurrentRoom { get; set; }
    public float Timer { get; set; }
}
