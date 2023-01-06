using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Scapped items Scriptable object, left for future
/// </summary>
[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public int attackStat;
    public int attackSpeedStat;
    public int speedStat;
    public int defenceStat;

    public Sprite sprite;
    public string blessing;

}
