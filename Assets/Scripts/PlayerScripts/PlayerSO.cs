using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Players Scriptable object containing player stats
/// </summary>
[CreateAssetMenu]
public class PlayerSO: ScriptableObject
{
    public float speed = 1.4f;
    public float attack = 1.4f;
    public float attackSpeed = 1.4f;
    public float knocbackMultiplier = 1.4f;

    public bool isSpeedChanged = false;
    public float baseSpeed = 1.4f;
    public float baseAttack = 1.4f;
    public float baseAttackSpeed = 1.4f;
    public float baseKnocbackMultiplier = 1.4f;


    public List<ItemSO> equipment;
    public int eqSpace = 4;
    public List<ItemSO> inventory;

    public float CurrentHealth = 5f;
    public float MaxHealth = 5f;

    public int secondaryAmmo = 5;
    public int maxSecondaryAmmo = 5;
}
