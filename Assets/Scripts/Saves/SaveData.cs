using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData 
{
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if(_current is null)
            {
                _current = new SaveData();
            }
            return _current;
        }

        set {
            _current = value;
        }
    }

    public PlayerDataSave player { get; set; }
    public int Seed { get; set; }
    public List<RoomsDataSave> rooms;
    public (int x, int y) CurrentRoom { get; set; }
    public float timer { get; set; }
}
