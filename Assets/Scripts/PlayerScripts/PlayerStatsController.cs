using BarsElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStatsController : MonoBehaviour
{
    public static PlayerStatsController instance;
    [SerializeField] private PlayerSO playerStats;

    [SerializeField] private UIDocument ui;
    [SerializeField] private Grid _layout;

    private Bar healthBar;
    private Bar secondaryBar;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playerStats.CurrentHealth = playerStats.MaxHealth;
        playerStats.secondaryAmmo = playerStats.maxSecondaryAmmo;
        var root = ui.rootVisualElement;
        healthBar = root.Q<Bar>("HealthBar");
        healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
        secondaryBar = root.Q<Bar>("SecondaryBar");
        secondaryBar.value = playerStats.secondaryAmmo;
    }

    public bool Heal()
    {
        if (playerStats.CurrentHealth < playerStats.MaxHealth)
        {
            playerStats.CurrentHealth += 1;
            healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
            return true;
        }
        return false;
    }

    public bool AmmoPickUp()
    {
        if (playerStats.secondaryAmmo < playerStats.maxSecondaryAmmo)
        {
            playerStats.secondaryAmmo += 1;
            secondaryBar.value = playerStats.secondaryAmmo;
            return true;
        }
        return false;
    }

    public bool UseSecondary()
    {
        if (playerStats.secondaryAmmo > 0)
        {
            playerStats.secondaryAmmo -= 1;
            secondaryBar.value = playerStats.secondaryAmmo;
            return true;
        }
        return false;
    }

    public void SetPlayerCords(int x, int y)
    {
       var pos =  _layout.CellToWorld(new Vector3Int(x + 1, -y + 1, 0));
        transform.position = pos;
    }

    public void SetPlayerBegginningStats()
    {
        playerStats.CurrentHealth = playerStats.MaxHealth;
        playerStats.secondaryAmmo = playerStats.maxSecondaryAmmo;
        playerStats.speed = playerStats.baseSpeed;
        playerStats.attackSpeed = playerStats.baseAttackSpeed;
        playerStats.attack = playerStats.baseAttack;
        playerStats.knocbackMultiplier = playerStats.baseKnocbackMultiplier;
        playerStats.equipment = new List<ItemSO>();
        playerStats.inventory = new List<ItemSO>();
        healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
        secondaryBar.value = playerStats.secondaryAmmo;
    }

    public float GetHealth()
    {
        return playerStats.CurrentHealth;
    }

    public int GetAmmo()
    {
        return playerStats.secondaryAmmo;
    }

    public void SetPlayerLoadedStats(float health, int ammo)
    {
        playerStats.CurrentHealth = health;
        playerStats.secondaryAmmo = ammo;
        playerStats.speed = playerStats.baseSpeed;
        playerStats.attackSpeed = playerStats.baseAttackSpeed;
        playerStats.attack = playerStats.baseAttack;
        playerStats.knocbackMultiplier = playerStats.baseKnocbackMultiplier;
        playerStats.equipment = new List<ItemSO>();
        playerStats.inventory = new List<ItemSO>();
        healthBar.value = (int)Mathf.Ceil(playerStats.CurrentHealth);
        secondaryBar.value = playerStats.secondaryAmmo;
    }
}
