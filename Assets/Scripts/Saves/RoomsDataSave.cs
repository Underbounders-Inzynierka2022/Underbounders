using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomsDataSave
{
    public int roomPosx;
    public int roomPosy;

    public List<(int x, int y)> chestOpened;
    public bool isConquered;
}
