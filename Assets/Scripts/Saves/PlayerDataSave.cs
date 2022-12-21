using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDataSave
{
    public int PlayerPosx { get; set; }
    public int PlayerPosy { get; set; }

    public float Health { get; set; }
    public int SecondaryAmmo { get; set; }
}
